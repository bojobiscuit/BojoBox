using BojoBox.Service;
using BojoBox.Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BojoBox.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // GET api/player/skater
        [HttpGet("test")]
        public JsonResult TestGet([FromQuery] StatParametersDto statParametersDto)
        {
            //return GetSeason(statParametersDto);
            //return GetSkater(825, statParametersDto);
            return GetCareer(statParametersDto);
        }

        // GET api/season
        [HttpGet("season")]
        public JsonResult GetSeason([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSeasonTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/career
        [HttpGet("career")]
        public JsonResult GetCareer([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetCareerTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/skater
        [HttpGet("skater/{id}")]
        public JsonResult GetSkater(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSkaterTable(id, statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/goalie
        [HttpGet("goalie/{id}")]
        public JsonResult GetGoalie(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            //WebsiteService service = new WebsiteService();
            //var dto = service.GetSkaterTable(id, statParametersDto);
            //return new JsonResult(dto);
            return null;
        }
    }
}
