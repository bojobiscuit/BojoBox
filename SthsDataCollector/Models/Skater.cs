using System.Collections.Generic;

namespace SthsDataCollector.Models
{
    public class Skater : Player
    {
        public SkaterSeasonStats SeasonTotals { get; set; }
        public IList<SkaterSeasonStats> SeasonSubTotals { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} minutes", Name, SeasonTotals.MP);
        }
    }
}
