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
        public IEnumerable<SkaterSeason> SkaterSeasons { get; set; }
        public IEnumerable<GoalieSeason> GoalieSeasons { get; set; }
    }
}
