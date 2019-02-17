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
            //return GetSkaterSeason(statParametersDto);
            //return GetSkater(825, statParametersDto);
            //return GetSkaterCareer(statParametersDto);
            //return GetGoalieSeason(statParametersDto);
            //return GetGoalieCareer(statParametersDto);
            return GetGoalie(129, statParametersDto);
        }

        // GET api/season
        [HttpGet("season/skater")]
        public JsonResult GetSkaterSeason([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSeasonSkaterTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/season
        [HttpGet("season/goalie")]
        public JsonResult GetGoalieSeason([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSeasonGoalieTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/career
        [HttpGet("career/skater")]
        public JsonResult GetSkaterCareer([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetCareerSkaterTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/career
        [HttpGet("career/goalie")]
        public JsonResult GetGoalieCareer([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetCareerGoalieTable(statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/skater
        [HttpGet("skater/{id}")]
        public JsonResult GetSkater(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetPlayerSkaterTable(id, statParametersDto);
            return new JsonResult(dto);
        }

        // GET api/goalie
        [HttpGet("goalie/{id}")]
        public JsonResult GetGoalie(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetPlayerGoalieTable(id, statParametersDto);
            return new JsonResult(dto);
        }
    }
}
