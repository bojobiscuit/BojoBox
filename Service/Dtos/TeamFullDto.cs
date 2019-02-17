using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class TeamFullDto : TeamDto
    {
        public Franchise FranchiseDto { get; set; }
        public IEnumerable<SkaterSeasonDto> SkaterSeasons { get; set; }
        public IEnumerable<GoalieSeasonDto> GoalieSeasons { get; set; }

        public new static TeamFullDto Create(Team source) => 
            AutoMapper.Mapper.Map<TeamFullDto>(source);
    }
}
