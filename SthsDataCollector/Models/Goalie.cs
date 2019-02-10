using System.Collections.Generic;

namespace SthsDataCollector.Models
{
    public class Goalie : Player
    {
        public GoalieSeasonStats SeasonTotals { get; set; }
        public IList<GoalieSeasonStats> SeasonSubTotals { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} minutes", Name, SeasonTotals.MP);
        }
    }
}
