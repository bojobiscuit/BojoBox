using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.EntityFramework.Entities
{
    public class Franchise
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public int CurrentTeamId { get; set; }

        public League League { get; set; }
        public Team CurrentTeam { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}
