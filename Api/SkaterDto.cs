using BojoBox.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class SkaterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public League League { get; set; }
        public IEnumerable<SkaterSeason> Seasons { get; set; }
    }
}
