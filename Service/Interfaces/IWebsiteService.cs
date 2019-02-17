using BojoBox.EntityFramework;
using BojoBox.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BojoBox.Service
{
    public interface IWebsiteService
    {
        // Season
        // Career
        // Player

        StatTableDto GetPlayerSkaterTable(int skaterId, StatParametersDto paramDto);
        StatTableDto GetSeasonSkaterTable(StatParametersDto paramDto);
        StatTableDto GetCareerSkaterTable(StatParametersDto paramDto);
    }
}
