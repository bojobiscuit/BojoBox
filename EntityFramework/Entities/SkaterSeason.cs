using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class SkaterSeason
    {
        public int Id { get; set; }
        public int SkaterId { get; set; }
        public int? TeamId { get; set; }
        public int? SubtotalForId { get; set; }

        public int Season { get; set; }
        public bool isPlayoffs { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }

        public Skater Skater { get; set; }
        public Team Team { get; set; }
        public SkaterSeason SubtotalFor { get; set; }
        public IEnumerable<SkaterSeason> SubTotals { get; set; }
    }
}

// Without Team
// Season: Where not subtotal
// Career: Where not subtotal and grouped by skater
// Player: Where not subtotal and skater matches

// By Team
// Season: Where team matches
// Career: Where team matches and grouped by skater
// Player: Where team matches and skater matches