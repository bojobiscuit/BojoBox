using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class SkaterDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static SkaterDto Create(Skater source) =>
            AutoMapper.Mapper.Map<SkaterDto>(source);
    }
}
