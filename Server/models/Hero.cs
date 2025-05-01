using System.Collections.Generic;

namespace stratzclone.Server.Models
{
    public class Hero
    {
        public int HeroId { get; set; }           // PK
        public string  name { get; set; }
        
        public string  localized_name { get; set; }

        public string pictureUrl { get; set; }

    }
}