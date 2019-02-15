using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class SkaterDto
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }

        public string Name { get; set; }

        public LeagueDto League { get; set; }
        public IEnumerable<SkaterSeasonDto> Seasons { get; set; }

        public static SkaterDto Create(Skater source) =>
            AutoMapper.Mapper.Map<SkaterDto>(source);
    }
}
