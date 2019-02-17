using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class SkaterSeasonFullDto : SkaterSeasonDto
    {
        public SkaterDto Skater { get; set; }
        public LeagueDto League { get; set; }
        public TeamDto Team { get; set; }

        public new static SkaterSeasonFullDto Create(SkaterSeason source) =>
            AutoMapper.Mapper.Map<SkaterSeasonFullDto>(source);
    }
}
