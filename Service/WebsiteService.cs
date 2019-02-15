using BojoBox.EntityFramework;
using BojoBox.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BojoBox.Service
{
    public class WebsiteService : IWebsiteService
    {
        public IEnumerable<SkaterDto> GetSkaters()
        {
            IEnumerable<SkaterDto> skaters = null;
            using (var db = new BojoBoxContext())
            {
                skaters = db.Skaters
                    //.Include(a => a.Seasons)
                    //.Include(a => a.League)
                    .Select(a => SkaterDto.Create(a))
                    .ToList();
            }
            return skaters;
        }
    }
}
