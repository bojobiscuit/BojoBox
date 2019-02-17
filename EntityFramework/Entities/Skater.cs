using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class Skater
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }

        public string Name { get; set; }

        public IEnumerable<SkaterSeason> Seasons { get; set; }
    }
}
