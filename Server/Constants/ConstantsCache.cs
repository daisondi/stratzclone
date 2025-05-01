// Server/Constants/ConstantsCache.cs
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;             // for IWebHostEnvironment
using StratzClone.Server.Interfaces;            // IConstantsCache
using StratzClone.Server.Models;                // Hero, Item

namespace StratzClone.Server.Constants
{
    public sealed class ConstantsCache : IConstantsCache
    {
        private readonly Dictionary<int, Item> _itemsById;
        private readonly Dictionary<int, Hero> _heroesById;

        public ConstantsCache(IWebHostEnvironment env)
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var constantsDir = Path.Combine(env.ContentRootPath, "constants");

            // Load items.json as a dictionary<string,Item>
            var itemsByKey = JsonSerializer.Deserialize<Dictionary<string, Item>>(
                File.ReadAllText(Path.Combine(constantsDir, "items.json")),
                opts)!;
            _itemsById = new Dictionary<int, Item>();
            foreach (var kv in itemsByKey)
                _itemsById[kv.Value.ItemId] = kv.Value;

            // Load heroes.json as dictionary<string,Hero>
            var heroesByKey = JsonSerializer.Deserialize<Dictionary<string, Hero>>(
                File.ReadAllText(Path.Combine(constantsDir, "heroes.json")),
                opts)!;
            _heroesById = new Dictionary<int, Hero>();
            foreach (var kv in heroesByKey)
                _heroesById[kv.Value.HeroId] = kv.Value;
        }

        public Item? GetItem(int id)
            => _itemsById.TryGetValue(id, out var item) ? item : null;

        public Hero? GetHero(int id)
            => _heroesById.TryGetValue(id, out var hero) ? hero : null;
    }
}
