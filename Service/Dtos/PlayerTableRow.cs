using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class PlayerTableRow
    {
        public int? Rank { get; set; }
        public SkaterDto Skater { get; set; }
        public TeamDto Team { get; set; }
        public int? Season { get; set; }
        public int? TeamCount { get; set; }
        public int? SeasonCount { get; set; }
        public IEnumerable<int> Stats { get; set; }
    }
}
