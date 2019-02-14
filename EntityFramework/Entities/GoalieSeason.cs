using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class GoalieSeason
    {
        public int Id {get; set;}
        public int GoalieId { get; set; }
        public int? TeamId { get; set; }
        public int? SubtotalForId { get; set; }

        public int Season { get; set; }
        public bool isPlayoffs { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Goalie Goalie { get; set; }
        public Team Team { get; set; }
        public GoalieSeason SubtotalFor { get; set; }
        public IEnumerable<GoalieSeason> SubTotals { get; set; }
    }
}
