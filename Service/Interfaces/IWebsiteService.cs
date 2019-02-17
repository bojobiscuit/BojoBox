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

        StatTableDto GetSkaterTable(int skaterId, StatParametersDto paramDto);
        StatTableDto GetSeasonTable(StatParametersDto paramDto);
        StatTableDto GetCareerTable(StatParametersDto paramDto);
    }
}
