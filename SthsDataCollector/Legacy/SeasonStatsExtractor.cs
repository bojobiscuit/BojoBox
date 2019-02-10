using SthsDataCollector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SthsDataCollector.Legacy
{
    public static class SeasonStatsExtractor
    {
        /// <summary>
        /// Returns a season worth of stats from a game file.
        /// Will only be neccessary when loading old season from SHL. 
        /// </summary>
        /// <param name="seasonNumber">Season number</param>
        /// <returns>Season object filled with stats</returns>
        public static Season ExtractSeason(int seasonNumber, bool isPlayoffs, string leagueAcronym)
        {
            string seasonType = isPlayoffs ?
                "Playoffs" : "Regular Season";

            // Load season file
            string filePath = GetFilePath(leagueAcronym, seasonNumber, isPlayoffs);
            if (!File.Exists(filePath))
                return null;
            SeasonFile seasonFile = new SeasonFile(filePath);

            // Pull data from season file
            Season season = new Season()
            {
                Number = seasonNumber,
                Type = seasonType,
                LeagueAcronym = leagueAcronym,
                Teams = new List<Team>(),
            };

            foreach (TeamSection teamSection in seasonFile.GetTeamSections())
                ExtractTeamFromSection(season, teamSection);

            return season;
        }

        private static void ExtractTeamFromSection(Season season, TeamSection teamSection)
        {
            int parenthesisIndex = teamSection.TeamInformationLine.IndexOf('(');
            Team team = new Team()
            {
                Name = teamSection.TeamInformationLine.Substring(0, parenthesisIndex).Replace(".", "").Trim().SplitCamelCase(),
                Acronym = teamSection.TeamInformationLine.GetAcronymns().First(),
                Skaters = new List<Skater>(),
                Goalies = new List<Goalie>(),
            };

            ExtractSkatersFromSection(team, teamSection);
            ExtractGoaliesFromSection(team, teamSection);
            RemoveFakePlayers(team);

            season.Teams.Add(team);
        }

        private static void ExtractSkatersFromSection(Team team, TeamSection teamSection)
        {
            foreach (string line in teamSection.SkaterInformationLines)
            {
                IEnumerable<string> acronyms = line.GetAcronymns();
                Skater skater = new Skater()
                {
                    Name = line.Substring(0, 30).RemoveAcronyms().Replace("_", "").Trim(),
                    IsRookie = acronyms.Contains("R"),
                    IsCaptain = acronyms.Contains("C"),
                    IsAlternate = acronyms.Contains("A"),
                };

                // Convert string of values into stats
                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ConvertRawStats(rawStats, skater);

                // Optional acronym found in traded players
                skater.SeasonTotals.TeamAcronym = Helper.GetTeamAcronym(acronyms);

                team.Skaters.Add(skater);
            }

            CombineSplitSkaterStats(team);
            CombineTradedSkaterSubtotals(team);
        }

        private static void ExtractGoaliesFromSection(Team team, TeamSection teamSection)
        {
            foreach (string line in teamSection.GoalieInformationLines)
            {
                IEnumerable<string> acronyms = line.GetAcronymns();
                Goalie goalie = new Goalie()
                {
                    Name = line.Substring(0, 30).RemoveAcronyms().Replace("_", "").Trim(),
                    IsRookie = acronyms.Contains("R"),
                    IsCaptain = acronyms.Contains("C"),
                    IsAlternate = acronyms.Contains("A"),
                };

                // Convert string of values into stats
                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ConvertRawStats(rawStats, goalie);

                // Optional acronym found in traded players
                goalie.SeasonTotals.TeamAcronym = Helper.GetTeamAcronym(acronyms);

                team.Goalies.Add(goalie);
            }

            CombineTradedGoalieSubtotals(team);
        }

        #region Helpers

        private static void ConvertRawStats(string[] rawStats, Skater skater)
        {
            double[] numericValues = rawStats.Select(stats => double.Parse(stats)).ToArray();
            SkaterSeasonStats seasonStats = new SkaterSeasonStats();

            // Some skater stats are split into two parts.
            // If there's less than 40 stats it's split in 2 parts
            if (numericValues.Length > 40)
            {
                #region Single Line Stats
                seasonStats.GP = (int)numericValues[0];
                seasonStats.G = (int)numericValues[1];
                seasonStats.A = (int)numericValues[2];
                seasonStats.P = (int)numericValues[3];
                seasonStats.PLMI = (int)numericValues[4];
                seasonStats.PIM = (int)numericValues[5];
                seasonStats.PM5 = (int)numericValues[6];
                seasonStats.HIT = (int)numericValues[7];
                seasonStats.HTT = (int)numericValues[8];
                seasonStats.SHT = (int)numericValues[9];
                seasonStats.OSB = (int)numericValues[10];
                seasonStats.OSM = (int)numericValues[11];
                //seasonStats.SPER = (int)numericValues[12]; Don't need percentage
                seasonStats.SB = (int)numericValues[13];
                seasonStats.MP = (int)numericValues[14];
                //seasonStats.AMG = (int)numericValues[15]; Don't need average
                seasonStats.PPG = (int)numericValues[16];
                seasonStats.PPA = (int)numericValues[17];
                seasonStats.PPP = (int)numericValues[18];
                seasonStats.PPS = (int)numericValues[19];
                seasonStats.PPM = (int)numericValues[20];
                seasonStats.PKG = (int)numericValues[21];
                seasonStats.PKA = (int)numericValues[22];
                seasonStats.PKP = (int)numericValues[23];
                seasonStats.PKS = (int)numericValues[24];
                seasonStats.PKM = (int)numericValues[25];
                seasonStats.GW = (int)numericValues[26];
                seasonStats.GT = (int)numericValues[27];
                //seasonStats.FOPer = (int)numericValues[28]; Don't need percentage
                seasonStats.FOT = (int)numericValues[29];
                seasonStats.FOW = Helper.GetPercentageAmount(numericValues[28], seasonStats.FOT);
                seasonStats.GA = (int)numericValues[30];
                seasonStats.TA = (int)numericValues[31];
                seasonStats.EG = (int)numericValues[32];
                seasonStats.HT = (int)numericValues[33];
                //skaseasonStatster.P20 = (int)numericValues[34]; Don't need average
                seasonStats.PSG = (int)numericValues[35];
                seasonStats.PSS = (int)numericValues[36];
                seasonStats.FW = (int)numericValues[37];
                seasonStats.FL = (int)numericValues[38];
                seasonStats.FT = (int)numericValues[39];
                seasonStats.GS = (int)numericValues[40];
                seasonStats.PS = (int)numericValues[41];
                seasonStats.WG = (int)numericValues[42];
                seasonStats.WP = (int)numericValues[43];

                // For some reason traded stats don't track stars. FUCK.
                if (numericValues.Length == 47)
                {
                    seasonStats.S1 = (int)numericValues[44];
                    seasonStats.S2 = (int)numericValues[45];
                    seasonStats.S3 = (int)numericValues[46];
                }
                else
                {
                    seasonStats.S1 = 0;
                    seasonStats.S2 = 0;
                    seasonStats.S3 = 0;
                }
                #endregion
            }
            else
            {
                #region Multi Line Stats
                if (numericValues.Length == 26)
                {
                    seasonStats.GP = (int)numericValues[0];
                    seasonStats.G = (int)numericValues[1];
                    seasonStats.A = (int)numericValues[2];
                    seasonStats.P = (int)numericValues[3];
                    seasonStats.PLMI = (int)numericValues[4];
                    seasonStats.PIM = (int)numericValues[5];
                    seasonStats.PM5 = (int)numericValues[6];
                    seasonStats.HIT = (int)numericValues[7];
                    seasonStats.HTT = (int)numericValues[8];
                    seasonStats.SHT = (int)numericValues[9];
                    seasonStats.OSB = (int)numericValues[10];
                    seasonStats.OSM = (int)numericValues[11];
                    //seasonStats.SPER = (int)numericValues[12]; Don't need percentage
                    seasonStats.SB = (int)numericValues[13];
                    seasonStats.MP = (int)numericValues[14];
                    //seasonStats.AMG = (int)numericValues[15]; Don't need average
                    seasonStats.PPG = (int)numericValues[16];
                    seasonStats.PPA = (int)numericValues[17];
                    seasonStats.PPP = (int)numericValues[18];
                    seasonStats.PPS = (int)numericValues[19];
                    seasonStats.PPM = (int)numericValues[20];
                    seasonStats.PKG = (int)numericValues[21];
                    seasonStats.PKA = (int)numericValues[22];
                    seasonStats.PKP = (int)numericValues[23];
                    seasonStats.PKS = (int)numericValues[24];
                    seasonStats.PKM = (int)numericValues[25];
                }
                else
                {
                    seasonStats.GW = (int)numericValues[0];
                    seasonStats.GT = (int)numericValues[1];
                    //seasonStats.FOPer = (int)numericValues[2]; Don't need percentage
                    seasonStats.FOT = (int)numericValues[3];
                    seasonStats.FOW = Helper.GetPercentageAmount(numericValues[2], seasonStats.FOT);
                    seasonStats.GA = (int)numericValues[4];
                    seasonStats.TA = (int)numericValues[5];
                    seasonStats.EG = (int)numericValues[6];
                    seasonStats.HT = (int)numericValues[7];
                    //skaseasonStatster.P20 = (int)numericValues[8]; Don't need average
                    seasonStats.PSG = (int)numericValues[9];
                    seasonStats.PSS = (int)numericValues[10];
                    seasonStats.FW = (int)numericValues[11];
                    seasonStats.FL = (int)numericValues[12];
                    seasonStats.FT = (int)numericValues[13];
                    seasonStats.GS = (int)numericValues[14];
                    seasonStats.PS = (int)numericValues[15];
                    seasonStats.WG = (int)numericValues[16];
                    seasonStats.WP = (int)numericValues[17];
                }
                #endregion
            }

            skater.SeasonTotals = seasonStats;
        }

        private static void ConvertRawStats(string[] rawStats, Goalie goalie)
        {
            double[] numericValues = rawStats.Select(stats => double.Parse(stats)).ToArray();
            GoalieSeasonStats seasonStats = new GoalieSeasonStats();

            seasonStats.GP = (int)numericValues[0];
            seasonStats.W = (int)numericValues[1];
            seasonStats.L = (int)numericValues[2];
            seasonStats.OTL = (int)numericValues[3];
            //seasonStats.PCT = (int)numericValues[4]; // Don't need percentage
            //seasonStats.GAA = (int)numericValues[5]; // Don't need percentage
            seasonStats.MP = (int)numericValues[6];
            seasonStats.PIM = (int)numericValues[7];
            seasonStats.SO = (int)numericValues[8];
            seasonStats.GA = (int)numericValues[9];
            seasonStats.SA = (int)numericValues[10];
            seasonStats.SAR = (int)numericValues[11];
            seasonStats.A = (int)numericValues[12];
            seasonStats.EG = (int)numericValues[13];
            //seasonStats.PSPer = (int)numericValues[14]; // Don't need percentage
            seasonStats.PSA = (int)numericValues[15];
            seasonStats.PSS = Helper.GetPercentageAmount(numericValues[14] * 100, seasonStats.PSA);
            seasonStats.ST = (int)numericValues[16];
            seasonStats.BG = (int)numericValues[17];
            if (numericValues.Length == 21)
            {
                seasonStats.S1 = (int)numericValues[18];
                seasonStats.S2 = (int)numericValues[19];
                seasonStats.S3 = (int)numericValues[20];
            }
            else
            {
                seasonStats.S1 = 0;
                seasonStats.S2 = 0;
                seasonStats.S3 = 0;
            }

            goalie.SeasonTotals = seasonStats;
        }

        private static void CombineSplitSkaterStats(Team team)
        {
            // 35 should be a safe number. Normally about 20 skaters on a team.
            // If there's roughly more than 30 then the stats are split
            if (team.Skaters.Count > 35)
            {
                int half = team.Skaters.Count / 2;
                for (int a = 0; a < half; a++)
                {
                    int b = a + half;
                    CombineSkaterStats(team.Skaters[a], team.Skaters[b]);
                }

                // Removes extra split skaters
                team.Skaters = team.Skaters.Take(half).ToList();
            }
        }

        private static void CombineTradedSkaterSubtotals(Team team)
        {
            // Traded skaters stored as new skaters are removed
            // Their season stats are pushed into the parent's subtotals
            List<Skater> skatersToDelete = new List<Skater>();
            Skater previousSkater = team.Skaters.FirstOrDefault();
            foreach (Skater skater in team.Skaters.Skip(1))
            {
                if (skater.Name == previousSkater.Name)
                {
                    skatersToDelete.Add(skater);
                    if (previousSkater.SeasonSubTotals == null)
                        previousSkater.SeasonSubTotals = new List<SkaterSeasonStats>();
                    previousSkater.SeasonSubTotals.Add(skater.SeasonTotals);
                }
                else
                    previousSkater = skater;
            }

            // Removes extra skaters
            team.Skaters = team.Skaters.Where(skater => !skatersToDelete.Contains(skater)).ToList();
        }

        private static void CombineTradedGoalieSubtotals(Team team)
        {
            // Traded goalies stored as new goalies are removed
            // Their season stats are pushed into the parent's subtotals
            List<Goalie> goaliesToDelete = new List<Goalie>();
            Goalie previousGoalie = team.Goalies.FirstOrDefault();
            foreach (Goalie goalie in team.Goalies.Skip(1))
            {
                if (goalie.Name == previousGoalie.Name)
                {
                    goaliesToDelete.Add(goalie);
                    if (previousGoalie.SeasonSubTotals == null)
                        previousGoalie.SeasonSubTotals = new List<GoalieSeasonStats>();
                    previousGoalie.SeasonSubTotals.Add(goalie.SeasonTotals);
                }
                else
                    previousGoalie = goalie;
            }

            // Removes extra goalies
            team.Goalies = team.Goalies.Where(goalie => !goaliesToDelete.Contains(goalie)).ToList();
        }

        private static void RemoveFakePlayers(Team team)
        {
            List<string> fakeNames = new List<string>()
            {
                "CPU Player"
            };

            // Removes fake players
            team.Skaters = team.Skaters.Where(skater => !fakeNames.Contains(skater.Name)).ToList();
            team.Goalies = team.Goalies.Where(goalie => !fakeNames.Contains(goalie.Name)).ToList();
        }

        private static void CombineSkaterStats(Skater skaterA, Skater skaterB)
        {
            skaterA.SeasonTotals.GW = skaterB.SeasonTotals.GW;
            skaterA.SeasonTotals.GT = skaterB.SeasonTotals.GT;
            skaterA.SeasonTotals.FOW = skaterB.SeasonTotals.FOW;
            skaterA.SeasonTotals.FOT = skaterB.SeasonTotals.FOT;
            skaterA.SeasonTotals.GA = skaterB.SeasonTotals.GA;
            skaterA.SeasonTotals.TA = skaterB.SeasonTotals.TA;
            skaterA.SeasonTotals.EG = skaterB.SeasonTotals.EG;
            skaterA.SeasonTotals.HT = skaterB.SeasonTotals.HT;
            skaterA.SeasonTotals.PSG = skaterB.SeasonTotals.PSG;
            skaterA.SeasonTotals.PSS = skaterB.SeasonTotals.PSS;
            skaterA.SeasonTotals.FW = skaterB.SeasonTotals.FW;
            skaterA.SeasonTotals.FL = skaterB.SeasonTotals.FL;
            skaterA.SeasonTotals.FT = skaterB.SeasonTotals.FT;
            skaterA.SeasonTotals.GS = skaterB.SeasonTotals.GS;
            skaterA.SeasonTotals.PS = skaterB.SeasonTotals.PS;
            skaterA.SeasonTotals.WG = skaterB.SeasonTotals.WG;
            skaterA.SeasonTotals.WP = skaterB.SeasonTotals.WP;
            skaterA.SeasonTotals.S1 = skaterB.SeasonTotals.S1;
            skaterA.SeasonTotals.S2 = skaterB.SeasonTotals.S2;
            skaterA.SeasonTotals.S3 = skaterB.SeasonTotals.S3;
        }

        private static string GetFilePath(string leagueAcronym, int season, bool isPlayoffs)
        {
            string seasonText = season.ToString();
            if (season < 10) seasonText = "0" + seasonText;

            string fileName = isPlayoffs ?
                $"{leagueAcronym} - PLF - Pro Team Scoring {seasonText}.html" :
                $"{leagueAcronym} - Pro Team Scoring {seasonText}.html";
            return Path.Combine(Environment.CurrentDirectory, @"LegacySeasonFiles\", fileName);
        }

        #endregion
    }
}
