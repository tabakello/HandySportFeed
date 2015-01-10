using System.Collections.Generic;
using System.Linq;
using HandySportFeed.Domain.Model;
using HtmlAgilityPack;

namespace HandySportFeed.Parsers.LiveScoreMachesParser {
    public class LiveScoreMatchesParserStrategy : IMatchesParserStrategy {
        public IEnumerable<Match> Parse(string url) {

            var web = new HtmlWeb();
            var doc = web.Load("http://www.livescore.com/soccer/2015-01-06/");
            List<HtmlNode> tables = doc.DocumentNode.Descendants().Where
             (x => (x.Name == "table" && x.Attributes["class"] != null &&
                x.Attributes["class"].Value.Contains("league-table"))).ToList();

            var matches = new List<Match>();

            foreach (var htmlNode in tables)
            {
                var rows = htmlNode.ChildNodes.Where(x => x.Name == "tr").ToList();
                var country = rows[0].ChildNodes.First(p => p.Name == "th").InnerText.Split(new[] { '-' })[0].Trim();
                var league = rows[0].ChildNodes.First(p => p.Name == "th").InnerText.Split(new[] { '-' })[1].Trim();

                rows.RemoveAt(0);
                foreach (var row in rows)
                {
                    var dateColumn = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fd").InnerText.Trim();
                    var homeCommandName = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fh").InnerText.Trim();
                    var score = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fs").InnerText.Trim();
                    int scoreHomeCommand, scoreAwayCommand;
                    if (score.Contains("?")) {
                        scoreHomeCommand = 0;
                        scoreAwayCommand = 0;
                    } else {
                         scoreHomeCommand = int.Parse(score.Split(new[] { '-' })[0].Trim());
                         scoreAwayCommand = int.Parse(score.Split(new[] { '-' })[1].Trim());
                    }
                    
                    var awayCommandName = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fa").InnerText.Trim();
                    if (dateColumn.Contains("&#x"))
                        dateColumn = "Live";

                    matches.Add(new Match {
                        AwayCommand = new Command { NameEn = awayCommandName },
                        HomeCommand = new Command { NameEn = homeCommandName },
                        ScoreAway = scoreAwayCommand,
                        ScoreHome = scoreHomeCommand,
                        Country = new Country { Name = country },
                        Tourney = new Tourney { LiveScoreName = league }
                    });
                }
            }

            return matches;
        }
    }
}