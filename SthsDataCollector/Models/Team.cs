using System.Collections.Generic;

namespace SthsDataCollector.Models
{
    public class Team
    {
        public string Name { get; set; }
        public string Acronym { get; set; }

        public IList<Skater> Skaters { get; set; }
        public IList<Goalie> Goalies { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Acronym);
        }
    }
}
