using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class SkaterFullDto : PlayerDto
    {
        public IEnumerable<SkaterSeasonDto> Seasons { get; set; }

        public new static SkaterFullDto Create(Skater source) =>
            AutoMapper.Mapper.Map<SkaterFullDto>(source);
    }
}
