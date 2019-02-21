using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class StatParametersDto
    {
        public int? Team { get; set; }
        public int? Era { get; set; }
        public int? Season { get; set; }
        public int? League { get; set; }
        public int? SeasonType { get; set; }
        public int? Col { get; set; }
        public int? Page { get; set; }
    }
}
