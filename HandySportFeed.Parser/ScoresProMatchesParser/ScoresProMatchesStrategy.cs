using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using HandySportFeed.Domain.Model;
using HandySportFeed.Parsers.Utils;

<<<<<<< HEAD
namespace HandySportFeed.Parsers.ScoresProMatchesParser
{
    public class ScoresProMatchesStrategy : IMatchesParserStrategy
    {
        public IEnumerable<Match> Parse()
        {
            //получаем xml
=======
namespace HandySportFeed.Parsers.ScoresProMatchesParser {
    public class ScoresProMatchesStrategy : IMatchesParserStrategy {
        public IEnumerable<Match> Parse() {
>>>>>>> 16b4f5a41b074ff196bc7ff6212dbc2303a6af71
            var xDoc = new XmlDocument();
            xDoc.LoadXml(XmlHelper.GetXmlStringByUrl(ConfigurationManager.AppSettings["ScoreProUrl"]));
            //достали матчи
            var listXmlMathces = xDoc.GetElementsByTagName("item");

            if (listXmlMathces.Count == 0) {
                throw new Exception("Alarm");
<<<<<<< HEAD
            //разделили мух от катлет
=======
            }

>>>>>>> 16b4f5a41b074ff196bc7ff6212dbc2303a6af71
            var scoreProMatches = ParseXmlMatches(listXmlMathces);
            //распарсили
            return ParseScoreProMathes(scoreProMatches);
        }

        private IEnumerable<ScoreProMatch> ParseXmlMatches(XmlNodeList listXmlMathces) {
            var scoreProMatches = new List<ScoreProMatch>();
            for (var i = 0; i < listXmlMathces.Count; i++) {
                var match = listXmlMathces[i];
                var titleNode = match.SelectSingleNode("title");
                var descriptionNode = match.SelectSingleNode("description");
                var pubDateNode = match.SelectSingleNode("pubDate");

                if (titleNode == null || descriptionNode == null || pubDateNode == null) {
                    throw new Exception("alarma");
                }

                scoreProMatches.Add(new ScoreProMatch {
                    Title = titleNode.InnerText,
                    Description = descriptionNode.InnerText,
                    PubDate = pubDateNode.InnerText
                });
            }
            return scoreProMatches;
        }

<<<<<<< HEAD
        private IEnumerable<Match> ParseScoreProMathes(IEnumerable<ScoreProMatch> scoreProMatches)
        {
            var matchList = new List<Match>();
            foreach (var scoreProMatch in scoreProMatches)
            {
                var values = scoreProMatch.Title.Split(new[] {':'});
=======
        private IEnumerable<Match> ParseScoreProMathes(IEnumerable<ScoreProMatch> scoreProMatches) {
            foreach (var scoreProMatch in scoreProMatches) {
                var values = scoreProMatch.Title.Split(new[] { ':' });
>>>>>>> 16b4f5a41b074ff196bc7ff6212dbc2303a6af71
                var tourney = values[1].Trim().Substring(values[1].IndexOf('('), values[1].IndexOf(')') - 2);
                var commands = values[1].Substring(values[1].IndexOf(')') + 1);
                var homeCommand = commands.Substring(0, commands.IndexOf(" vs ", StringComparison.Ordinal)).Trim();
                var awayCommand = commands.Substring(commands.IndexOf(" vs ", StringComparison.Ordinal) + 3).Trim();
<<<<<<< HEAD
                var scoreHomeCommand = int.Parse(values[2].Trim().Split(new[] { '-' })[0]);
                var scoreAwayCommand = int.Parse(values[2].Trim().Split(new[] { '-' })[1]);
                matchList.Add(new Match
                {
                    ScoreAway = scoreAwayCommand,
                    ScoreHome = scoreHomeCommand,
                    //HomeCommand = Commands.Where(p => p.Name == "homeCommand")
                });
            }
            return matchList;
=======
                var scoreHomeCommand = values[2].Trim().Split(new[] { '-' })[0];
                var scoreAwayCommand = values[2].Trim().Split(new[] { '-' })[1];
            }

            return new List<Match> {
                new Match { AwayCommand = new Command { Id = 1, NameEn = "Sevastopol" } },
                new Match { AwayCommand = new Command { Id = 1, NameEn = "Simferopol" } }
            };
>>>>>>> 16b4f5a41b074ff196bc7ff6212dbc2303a6af71
        }
    }
}
