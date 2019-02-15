using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class LeagueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public IEnumerable<FranchiseDto> Franchises { get; set; }
        public IEnumerable<SkaterDto> Skaters { get; set; }
        public IEnumerable<GoalieDto> Goalies { get; set; }

        public static LeagueDto Create(League source) =>
            AutoMapper.Mapper.Map<LeagueDto>(source);
    }
}
