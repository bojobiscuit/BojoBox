using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.SthsDataCollector.Moderno
{
    public class SeasonData
    {
        public int SeasonNumber { get; private set; }
        public bool IsPlayoffs { get; set; }
        public string LeagueAcronym { get; private set; }
        public List<TeamInfo> Teams { get; set; }
        public List<SkaterRow> SkaterRows { get; set; }
        public List<GoalieRow> GoalieRows { get; set; }

        public SeasonData(int seasonNumber, string leagueAcronym, bool isPlayoffs)
        {
            SeasonNumber = seasonNumber;
            LeagueAcronym = leagueAcronym;
            IsPlayoffs = isPlayoffs;
            Teams = new List<TeamInfo>();
            SkaterRows = new List<SkaterRow>();
            GoalieRows = new List<GoalieRow>();
        }
    }
}
