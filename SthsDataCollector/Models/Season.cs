using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SthsDataCollector.Models
{
    public class Season
    {
        public IList<Team> Teams { get; set; }

        public int Number { get; set; }

        public string Type { get; set; }

        public string LeagueAcronym { get; set; }

        public override string ToString()
        {
            return string.Format("#{0} - {1} teams", Number, Teams.Count);
        }

    }
}
