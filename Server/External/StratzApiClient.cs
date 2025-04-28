using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using stratzclone.Server.Interfaces;
using stratzclone.Server.Models;
using stratzclone.Server.External;

namespace stratzclone.Server.External
{
    public class StratzApiClient : IStratzApiClient
    {
        private readonly HttpClient _http;
        private const int DefaultSkip = 0;
        private const int DefaultTake = 200;

        public StratzApiClient(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }


        public async Task<IEnumerable<Match>> GetRecentMatchesAsync(string steamId)
        {
            // parse to a Long for GraphQL
            if (!long.TryParse(steamId, out var accountId))
                throw new ArgumentException("Invalid Steam64 ID", nameof(steamId));

            // build payload
            var payload = new
            {
                query = GraphQLQueries.GetPlayerMatches,
                variables = new
                {
                    steamAccountId = accountId,
                    skip = DefaultSkip,
                    take = DefaultTake
                }
            };

            // POST to /graphql
            var request = new HttpRequestMessage(HttpMethod.Post, "graphql")
            {
                Content = JsonContent.Create(payload)
            };
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            var resp = await _http.SendAsync(request);
            resp.EnsureSuccessStatusCode();

            // unwrap GraphQL envelope
            var gql = await resp.Content
                .ReadFromJsonAsync<GraphQLResponse<PlayerMatchesData>>();
            var matchesDto = gql?.Data?.Player?.Matches
                             ?? Array.Empty<GraphQLMatchDto>();

            // map to your domain
            var outList = new List<Match>();
            foreach (var m in matchesDto)
            {
                var match = new Match
                {
                    MatchId = m.Id,
                    DurationSecs = m.DurationSeconds,
                    DidRadiantWin = m.DidRadiantWin,
                    // convert the epoch → DateTime:
                    StartDateUtc = DateTimeOffset
                    .FromUnixTimeSeconds(m.StartDateTime)
                    .UtcDateTime
                };


                foreach (var p in m.Players)
                {
                    var pm = new PlayerMatch
                    {
                        MatchId = match.MatchId,
                        SteamId = p.SteamId,
                        HeroId = p.HeroId,
                        Kills = p.Kills,
                        Deaths = p.Deaths,
                        Assists = p.Assists
                    };

                    foreach (var it in p.Items)
                    {
                        pm.Items.Add(new PlayerMatchItem
                        {
                            MatchId = match.MatchId,
                            SteamId = pm.SteamId,
                            ItemSeq = it.Sequence,
                            ItemId = it.ItemId,
                            PurchaseTime = it.Time,
                            IsNeutral = it.Neutral,
                            Charges = it.Charges
                        });
                    }

                    match.PlayerMatches.Add(pm);
                }

                outList.Add(match);
            }

            return outList;
        }
         public async Task<IEnumerable<PlayerMatch>> GetPlayerMatchesAsync(
        string steamId,
        int skip = 0,
        int take = 100
    )
    {
        if (!long.TryParse(steamId, out var accountId))
            throw new ArgumentException("Invalid Steam64 ID", nameof(steamId));

        var payload = new
        {
            query = GraphQLQueries.GetPlayerMatches,
            variables = new
            {
                steamAccountId = accountId,
                skip,
                take
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "graphql")
        {
            Content = JsonContent.Create(payload)
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var resp = await _http.SendAsync(request);
        resp.EnsureSuccessStatusCode();

        var gql = await resp.Content
            .ReadFromJsonAsync<GraphQLResponse<PlayerMatchesData>>();

        var matchesDto = gql?.Data?.Player?.Matches
            ?? Array.Empty<GraphQLMatchDto>();

        // Flatten out only the PlayerMatch records
        var playerMatches = new List<PlayerMatch>();
        foreach (var m in matchesDto)
        {
            foreach (var p in m.Players)
            {
                playerMatches.Add(new PlayerMatch
                {
                    MatchId = m.Id,
                    SteamId = p.SteamId,
                    IsRadiant  = p.IsRadiant, 
                    HeroId = p.HeroId,
                    Kills = p.Kills,
                    Deaths = p.Deaths,
                    Assists = p.Assists
                });
            }
        }

        return playerMatches;
    }

        private class GraphQLResponse<T>
        {
            public T Data { get; set; } = default!;
        }

        private class PlayerMatchesData
        {
            public PlayerContainer Player { get; set; } = default!;
        }

        private class PlayerContainer
        {
            public GraphQLMatchDto[] Matches { get; set; }
                = Array.Empty<GraphQLMatchDto>();
        }

        private class GraphQLMatchDto
        {
            public long Id { get; set; }
            public bool DidRadiantWin { get; set; }
            public int DurationSeconds { get; set; }

            // Raw epoch seconds or milliseconds (depending on Stratz)
            // Matches the JSON numeric token
            public long StartDateTime { get; set; }

            public GraphQLPlayerDto[] Players { get; set; } = Array.Empty<GraphQLPlayerDto>();
        }


        private class GraphQLPlayerDto
        {
            public string SteamId { get; set; } = default!;
            
            public bool IsRadiant { get; set; }   
            public int HeroId { get; set; }
            public int Kills { get; set; }
            public int Deaths { get; set; }
            public int Assists { get; set; }
            public GraphQLItemDto[] Items { get; set; }
                = Array.Empty<GraphQLItemDto>();
        }

        private class GraphQLItemDto
        {
            public int Sequence { get; set; }
            public int ItemId { get; set; }
            public int Time { get; set; }
            public bool Neutral { get; set; }
            public int? Charges { get; set; }
        }
    }
}
