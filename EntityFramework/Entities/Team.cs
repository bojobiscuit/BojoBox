using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.EntityFramework.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public int? FranchiseId { get; set; }

        public string Name { get; set; }
        public string Acronym { get; set; }

        public Franchise Franchise { get; set; }
        public IEnumerable<SkaterSeason> SkaterSeasons { get; set; }
        public IEnumerable<GoalieSeason> GoalieSeasons { get; set; }
    }
}
