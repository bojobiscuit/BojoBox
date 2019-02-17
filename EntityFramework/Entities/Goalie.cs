using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class Goalie
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }

        public string Name { get; set; }
        
        public IEnumerable<GoalieSeason> Seasons { get; set; }
    }
}
