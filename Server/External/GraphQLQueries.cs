namespace stratzclone.Server.External
{
    public static class GraphQLQueries
    {
         public const string GetPlayerMatches = @"
query GetPlayerMatches($steamAccountId: Long!, $skip: Int = 0, $take: Int = 100){
  player(steamAccountId:$steamAccountId){
    matches(request:{skip:$skip,take:$take}){
      id
      didRadiantWin
      durationSeconds
      startDateTime
      players {
  			steamAccountId
        heroId
        kills
        deaths
        assists
        isRadiant
        item0Id
        item1Id
        item2Id
        item3Id
        item4Id
        item5Id
    }
  }
}
}";
    }
}
