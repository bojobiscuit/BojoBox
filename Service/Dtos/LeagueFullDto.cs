using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class LeagueFullDto : LeagueDto
    {
        public IEnumerable<FranchiseDto> Franchises { get; set; }
        public IEnumerable<SkaterDto> Skaters { get; set; }
        public IEnumerable<GoalieDto> Goalies { get; set; }

        public new static LeagueFullDto Create(League source) =>
            AutoMapper.Mapper.Map<LeagueFullDto>(source);
    }
}
