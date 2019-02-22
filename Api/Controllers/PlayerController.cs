using BojoBox.Service;
using BojoBox.Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BojoBox.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        [HttpGet("search/{input}")]
        public JsonResult GetSearchResults(string input)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSearchResults(input);
            return new JsonResult(dto);
        }

        [HttpGet("season/skater")]
        public JsonResult GetSkaterSeason([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSeasonSkaterTable(statParametersDto);
            return new JsonResult(dto);
        }

        [HttpGet("season/goalie")]
        public JsonResult GetGoalieSeason([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetSeasonGoalieTable(statParametersDto);
            return new JsonResult(dto);
        }

        [HttpGet("career/skater")]
        public JsonResult GetSkaterCareer([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetCareerSkaterTable(statParametersDto);
            return new JsonResult(dto);
        }

        [HttpGet("career/goalie")]
        public JsonResult GetGoalieCareer([FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetCareerGoalieTable(statParametersDto);
            return new JsonResult(dto);
        }

        [HttpGet("skater/{id}")]
        public JsonResult GetSkater(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetPlayerSkaterTable(id, statParametersDto);
            return new JsonResult(dto);
        }

        [HttpGet("goalie/{id}")]
        public JsonResult GetGoalie(int id, [FromQuery] StatParametersDto statParametersDto)
        {
            WebsiteService service = new WebsiteService();
            var dto = service.GetPlayerGoalieTable(id, statParametersDto);
            return new JsonResult(dto);
        }
    }
}
