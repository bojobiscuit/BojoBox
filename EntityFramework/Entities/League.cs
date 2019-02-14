using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.EntityFramework.Entities
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public IEnumerable<Franchise> Franchises { get; set; }
        public IEnumerable<Skater> Skaters { get; set; }
        public IEnumerable<Goalie> Goalies { get; set; }
    }
}
