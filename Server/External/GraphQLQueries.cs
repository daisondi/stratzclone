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
    }
  }
}";
    }
}
