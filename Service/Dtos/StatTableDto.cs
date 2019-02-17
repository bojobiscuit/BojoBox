using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class StatTableDto
    {
        public string DisplayType { get; set; }
        public string PlayerType { get; set; }
        public string HeaderText { get; set; }

        public IEnumerable<int> Seasons { get; set; }
        public IEnumerable<TeamDto> Teams { get; set; }
        public StatParametersDto StatParameters { get; set; }

        public IEnumerable<PlayerTableRow> PlayerRows { get; set; }
        public IEnumerable<int> Totals { get; set; }
    }
}
