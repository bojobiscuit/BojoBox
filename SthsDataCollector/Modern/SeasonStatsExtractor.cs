using HtmlAgilityPack;
using SthsDataCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SthsDataCollector.Modern
{
    public class SeasonStatsExtractor
    {
        /// <summary>
        /// Returns a season worth of stats from a game file.
        /// </summary>
        /// <param name="urlPattern">url of the scoring files where the season number is replaced by {0} for easy patterns</param>
        /// <param name="seasonNumber">season number to fill into the {0}</param>
        /// <param name="leagueAcronym">acronym of the league used in the url</param>
        /// <returns>Season object filled with stats</returns>
        public static Season ExtractSeason(string urlPattern, int seasonNumber, bool isPlayoffs, string leagueAcronym)
        {
            string seasonType = isPlayoffs ? "Playoffs" : "Regular Season";

            // Load season file
            string url = string.Format(urlPattern, seasonNumber, leagueAcronym);
            SeasonFile seasonFile = new SeasonFile(url, seasonNumber, leagueAcronym, isPlayoffs);

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
            string acronym = teamSection.TeamInformationNode.Attributes["class"].Value.Replace("TeamScoringPro_", "");
            string name = teamSection.TeamInformationNode.FirstChild.Id.SplitCamelCase().Trim();
            if (name.Length < 2)
                name = teamSection.TeamInformationNode.FirstChild.GetAttributeValue("name", "ERROR").SplitCamelCase();
            name = name.Replace(".", "");

            Team team = new Team()
            {
                Name = name,
                Acronym = acronym,
                Skaters = new List<Skater>(),
                Goalies = new List<Goalie>(),
            };

            ExtractSkatersFromSection(team, teamSection);
            ExtractGoaliesFromSection(team, teamSection);

            RemoveFakePlayers(team);
            RemoveTradedSeasonsForFreeAgents(team);
            RemoveSkatersWhoDontPlay(team);

            season.Teams.Add(team);
        }

        private static void ExtractSkatersFromSection(Team team, TeamSection teamSection)
        {
            // First Skater Stat Section
            foreach (HtmlNode row in teamSection.SkaterInformationNode1.Descendants("tr").Where(a => a.ParentNode.Name != "thead"))
            {
                IList<HtmlNode> cells = row.Descendants("td").ToList();

                string name = cells.First().InnerText;
                bool isTraded = name.StartsWith("_");
                name = name.Replace("_", "").Trim();
                IEnumerable<string> acronyms = name.GetAcronymns();

                Skater skater = new Skater()
                {
                    Name = name.RemoveAcronyms(),
                    IsRookie = acronyms.Contains("R"),
                    IsCaptain = acronyms.Contains("C"),
                    IsAlternate = acronyms.Contains("A"),
                };

                ConvertCellsToStats(cells, skater);

                if (isTraded)
                    skater.SeasonTotals.TeamAcronym = acronyms.Where(a => a.Length == 3).First();

                team.Skaters.Add(skater);
            }

            // Second Skater Stat Section
            foreach (HtmlNode row in teamSection.SkaterInformationNode2.Descendants("tr").Where(a => a.ParentNode.Name != "thead"))
            {
                IList<HtmlNode> cells = row.Descendants("td").ToList();
                string name = cells.First().InnerText.Replace("_", "").Trim();
                IEnumerable<string> acronyms = name.GetAcronymns();

                Skater skater = new Skater()
                {
                    Name = name.RemoveAcronyms(),
                };

                FillInBlankColumnsWithZeros(cells);
                ConvertCellsToStats(cells, skater);
                team.Skaters.Add(skater);
            }

            CombineSplitSkaterStats(team);
            CombineTradedSkaterSubtotals(team);
        }

        private static void ExtractGoaliesFromSection(Team team, TeamSection teamSection)
        {
            // Goalie Stat Section
            foreach (HtmlNode row in teamSection.GoalieInformationNode.Descendants("tr").Where(a => a.ParentNode.Name != "thead"))
            {
                IList<HtmlNode> cells = row.Descendants("td").ToList();

                string name = cells.First().InnerText;
                bool isTraded = name.StartsWith("_");
                name = name.Replace("_", "").Trim();
                IEnumerable<string> acronyms = name.GetAcronymns();

                Goalie goalie = new Goalie()
                {
                    Name = name.RemoveAcronyms(),
                    IsRookie = acronyms.Contains("R"),
                    IsCaptain = acronyms.Contains("C"),
                    IsAlternate = acronyms.Contains("A"),
                };

                ConvertCellsToStats(cells, goalie);

                if (isTraded)
                    goalie.SeasonTotals.TeamAcronym = acronyms.Where(a => a.Length == 3).First();

                team.Goalies.Add(goalie);
            }

            CombineTradedGoalieSubtotals(team);
        }

        #region Helper Methods

        private static void ConvertCellsToStats(IList<HtmlNode> cells, Skater skater)
        {
            double[] numericValues = cells.Skip(3).Select(cell => double.Parse(cell.InnerText.Replace("%", ""))).ToArray();
            SkaterSeasonStats seasonStats = new SkaterSeasonStats();

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

                // For some reason traded stats don't track stars. FUCK.
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
            }

            skater.SeasonTotals = seasonStats;
        }

        private static void ConvertCellsToStats(IList<HtmlNode> cells, Goalie goalie)
        {
            double[] numericValues = cells.Skip(1).Select(cell => double.Parse(cell.InnerText.Replace("%", ""))).ToArray();
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
            // Before a certain point, AI (fake) players were added to fill out rosters.
            // We don't want these assholes in the stats

            List<string> fakeNames = new List<string>()
            {
                "CPU"
            };

            // Removes fake players
            team.Skaters = team.Skaters.Where(skater => !fakeNames.Contains(skater.Name)).ToList();
            team.Goalies = team.Goalies.Where(goalie => !fakeNames.Contains(goalie.Name)).ToList();
        }

        private static void RemoveTradedSeasonsForFreeAgents(Team team)
        {
            // Subtotals get added to a player's subtotal list if they have more than one row of stats for a season.
            // However, Free Agents apparently have this redundant row that matches the total
            // We'll just null any subtotals that have less than 2 items.

            foreach (var skater in team.Skaters)
            {
                if (skater.SeasonSubTotals != null && skater.SeasonSubTotals.Count < 2)
                    skater.SeasonSubTotals = null;
            }
            foreach (var goalie in team.Goalies)
            {
                if (goalie.SeasonSubTotals != null && goalie.SeasonSubTotals.Count < 2)
                    goalie.SeasonSubTotals = null;
            }
        }

        private static void RemoveSkatersWhoDontPlay(Team team)
        {
            // Sometimes players are added to a roster if they're part of the franchise (like NHL -> AHL)
            // However, even if they don't have any stats they get added to the roster
            // Let's just remove all players that average less than 3 minutes a game.
            // Sometimes they "play" for about a minute a game by accident.
            int minutesPerGameMin = 3;
            team.Skaters = team.Skaters
                .Where(skater => skater.SeasonTotals.MP > (skater.SeasonTotals.GP * minutesPerGameMin))
                .ToList();
        }

        private static void FillInBlankColumnsWithZeros(IList<HtmlNode> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.InnerText.Trim() == "")
                    cell.InnerHtml = "0";
            }
        }

        #endregion
    }
}
