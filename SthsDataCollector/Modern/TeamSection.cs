using HtmlAgilityPack;
using System;
using System.Linq;

namespace SthsDataCollector.Modern
{
    internal class TeamSection
    {
        public HtmlNode TeamInformationNode { get; set; }
        public HtmlNode SkaterInformationNode1 { get; set; }
        public HtmlNode SkaterInformationNode2 { get; set; }
        public HtmlNode GoalieInformationNode { get; set; }

        public TeamSection(HtmlNode teamInfoNode, HtmlNode playerInformationContainerNode)
        {
            TeamInformationNode = teamInfoNode;
            if (playerInformationContainerNode.InnerHtml.Contains("STHSScoring_PlayersTable2"))
            {
                SkaterInformationNode1 = GetPlayerInfoTable(playerInformationContainerNode, "STHSScoring_PlayersTable1");
                SkaterInformationNode2 = GetPlayerInfoTable(playerInformationContainerNode, "STHSScoring_PlayersTable2");
                GoalieInformationNode = GetPlayerInfoTable(playerInformationContainerNode, "STHSScoring_GoaliesTable");
            }
            else
            {
                var tables = playerInformationContainerNode.Descendants("table").Where(a => a.HasClass("basictablesorter")).ToArray();
                SkaterInformationNode1 = tables[0];
                SkaterInformationNode2 = tables[1];
                GoalieInformationNode = tables[2];
            }
        }

        public TeamSection(HtmlNode teamInfoNode, HtmlNode table1, HtmlNode table2, HtmlNode table3)
        {
            TeamInformationNode = teamInfoNode;
            SkaterInformationNode1 = table1;
            SkaterInformationNode2 = table2;
            GoalieInformationNode = table3;
        }

        private HtmlNode GetPlayerInfoTable(HtmlNode playerInformationContainerNode, string className)
        {
            return playerInformationContainerNode.Descendants("table").Where(a => a.HasClass(className)).First();
        }
    }
}
