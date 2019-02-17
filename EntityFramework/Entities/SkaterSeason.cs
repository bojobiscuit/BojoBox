using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class SkaterSeason
    {
        public int Id { get; set; }
        public int SkaterId { get; set; }
        public int LeagueId { get; set; }
        public int? TeamId { get; set; }
        public int? SubtotalForId { get; set; }

        public int Season { get; set; }
        public bool isPlayoffs { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int PlusMinus { get; set; }
        public int PenaltyMinutes { get; set; }
        public int PenaltyMajors { get; set; }
        public int Hits { get; set; }
        public int HitsTaken { get; set; }
        public int Shots { get; set; }
        public int OwnShotsBlocked { get; set; }
        public int OwnShotsMissed { get; set; }
        public int ShotsBlocked { get; set; }
        public int MinutesPlayed { get; set; }
        public int PPGoals { get; set; }
        public int PPAssists { get; set; }
        public int PPPoints { get; set; }
        public int PPShots { get; set; }
        public int PPMinutes { get; set; }
        public int PKGoals { get; set; }
        public int PKAssists { get; set; }
        public int PKPoints { get; set; }
        public int PKShots { get; set; }
        public int PKMinutes { get; set; }
        public int GameWinningGoals { get; set; }
        public int GameTyingGoals { get; set; }
        public int FaceoffsTotal { get; set; }
        public int EmptyNetGoals { get; set; }
        public int HatTricks { get; set; }
        public int PenaltyShotGoals { get; set; }
        public int PenaltyShotAttempts { get; set; }
        public int FightsWon { get; set; }
        public int FightsLost { get; set; }
        public int FightsDraw { get; set; }
        public int FaceoffWins { get; set; }

        public Skater Skater { get; set; }
        public League League { get; set; }
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