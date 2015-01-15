using System;
using System.Collections.Generic;
using System.Xml;
using HandySportFeed.Domain.Model;
using HandySportFeed.Parsers.Utils;


namespace HandySportFeed.Parsers.ScoresProMatchesParser
{
    public class ScoresProMatchesStrategy : IMatchesParserStrategy
    {
        public IEnumerable<Match> Parse(string url,DateTime date)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(XmlHelper.GetXmlStringByUrl(url));
            //достали матчи
            var listXmlMathces = xDoc.GetElementsByTagName("item");

            if (listXmlMathces.Count == 0)
            {
                throw new Exception("Alarm");
            }

            //разделили мух от катлет
            var scoreProMatches = ParseXmlMatches(listXmlMathces);
            //распарсили
            return ParseScoreProMathes(scoreProMatches);
        }

        public IEnumerable<Match> GetTodayMatches(string url) {
            throw new NotImplementedException();
        }

        public IEnumerable<Match> GetMatchesByDate(string url, DateTime date) {
            throw new NotImplementedException();
        }

        private IEnumerable<ScoreProMatch> ParseXmlMatches(XmlNodeList listXmlMathces)
        {
            var scoreProMatches = new List<ScoreProMatch>();
            for (var i = 0; i < listXmlMathces.Count; i++)
            {
                var match = listXmlMathces[i];
                var titleNode = match.SelectSingleNode("title");
                var descriptionNode = match.SelectSingleNode("description");
                var pubDateNode = match.SelectSingleNode("pubDate");

                if (titleNode == null || descriptionNode == null || pubDateNode == null)
                {
                    throw new Exception("alarma");
                }

                scoreProMatches.Add(new ScoreProMatch
                {
                    Title = titleNode.InnerText,
                    Description = descriptionNode.InnerText,
                    PubDate = pubDateNode.InnerText
                });
            }
            return scoreProMatches;
        }

        private IEnumerable<Match> ParseScoreProMathes(IEnumerable<ScoreProMatch> scoreProMatches)
        {
            var matchList = new List<Match>();
            foreach (var scoreProMatch in scoreProMatches)
            {
                var values = scoreProMatch.Title.Split(new[] { ':' });

                var tourney = values[1].Trim().Substring(values[1].IndexOf('('), values[1].IndexOf(')') - 2);
                var commands = values[1].Substring(values[1].IndexOf(')') + 1);
                var homeCommand = commands.Substring(0, commands.IndexOf(" vs ", StringComparison.Ordinal)).Trim();
                var awayCommand = commands.Substring(commands.IndexOf(" vs ", StringComparison.Ordinal) + 3).Trim();
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
        }
    }
}
