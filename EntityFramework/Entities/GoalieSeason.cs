using System.Collections.Generic;

namespace BojoBox.EntityFramework.Entities
{
    public class GoalieSeason
    {
        public int Id { get; set; }
        public int GoalieId { get; set; }
        public int LeagueId { get; set; }
        public int? TeamId { get; set; }
        public int? SubtotalForId { get; set; }

        public int Season { get; set; }
        public bool isPlayoffs { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int OvertimeLosses { get; set; }
        public int Minutes { get; set; }
        public int PenaltyMinutes { get; set; }
        public int Shutouts { get; set; }
        public int GoalsAgainst { get; set; }
        public int ShotsAgainst { get; set; }
        public int Assists { get; set; }
        public int EmptyGoalAgainst { get; set; }
        public int PenaltyShotAttempts { get; set; }
        public int Starts { get; set; }
        public int Backups { get; set; }
        public int PenaltyShotSaves { get; set; }

        public Goalie Goalie { get; set; }
        public League League { get; set; }
        public Team Team { get; set; }
        public GoalieSeason SubtotalFor { get; set; }
        public IEnumerable<GoalieSeason> SubTotals { get; set; }
    }
}
