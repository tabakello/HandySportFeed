using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HandySportFeed.Domain.Model;
using HtmlAgilityPack;

namespace HandySportFeed.Parsers.LiveScoreMachesParser {
    public class LiveScoreMatchesParserStrategy : IMatchesParserStrategy {
        public IEnumerable<Match> Parse(string url, DateTime date) {
            var matches = new List<Match>();
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://www.livescore.com/soccer/{0}-{1}-{2}/", date.Year, date.Month, date.Day));
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/17.0";
            request.AllowAutoRedirect = true;
            request.Referer = "http://www.livescore.com/soccer/";
            request.Date = DateTime.UtcNow;
            request.Headers.Add("Accept-Language", "ru");

            var response = (HttpWebResponse)request.GetResponse();
            var html = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> tables = doc.DocumentNode.Descendants().Where
             (x => (x.Name == "table" && x.Attributes["class"] != null &&
                x.Attributes["class"].Value.Contains("league-table"))).ToList();
            
            foreach (var htmlNode in tables) {
                var rows = htmlNode.ChildNodes.Where(x => x.Name == "tr").ToList();
                var country = rows[0].ChildNodes.First(p => p.Name == "th").InnerText.Split(new[] { '-' })[0].Trim();

                var league = rows[0].ChildNodes.First(p => p.Name == "th").ChildNodes[1].InnerText.Split(new[] { '-' })[1].Trim();
                rows.RemoveAt(0);

                foreach (var row in rows) {
                    var dateColumn = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fd").InnerText.Trim();
                    var liveMinute = string.Empty;
                    var matchStatus = "NS";
                    if (dateColumn.Contains("&#x27")) {
                        matchStatus = "Live ";
                        liveMinute = dateColumn.Replace("&#x27", "'");
                    }
                       
                    if (dateColumn == "FT" || dateColumn == "HT")
                        matchStatus = dateColumn;
                    if (dateColumn == "Postp.")
                        matchStatus = "Canceled";

                    var matchDate = date.AddHours(3);
                    if (matchStatus == "NS")
                    {
                        var timeArray = dateColumn.Split(new[] { ':' });
                        matchDate = (date.Date + new TimeSpan(int.Parse(timeArray[0]), int.Parse(timeArray[1]), 0)).AddHours(3);
                        if(matchDate.Date > date.Date)
                            continue;
                    }
                    if (matchStatus == "Canceled") {
                        matchDate = date.Date + new TimeSpan(0, 0, 0);
                    }

                    var homeCommandName = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fh").InnerText.Trim();
                    var awayCommandName = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fa").InnerText.Trim();

                    var score = row.ChildNodes.First(p => p.Attributes["class"] != null && p.Attributes["class"].Value == "fs").InnerText.Trim();
                    int scoreHomeCommand, scoreAwayCommand;
                    if (score.Contains("?")) {
                        scoreHomeCommand = 0;
                        scoreAwayCommand = 0;
                    } else {
                        scoreHomeCommand = int.Parse(score.Split(new[] { '-' })[0].Trim());
                        scoreAwayCommand = int.Parse(score.Split(new[] { '-' })[1].Trim());
                    }

                    matches.Add(new Match {
                        ScoreAway = scoreAwayCommand,
                        ScoreHome = scoreHomeCommand,
                        Date = matchDate,
                        AwayCommand = new Command { LiveScoreName = awayCommandName },
                        HomeCommand = new Command { LiveScoreName = homeCommandName },
                        Country = new Country { Name = country },
                        Tourney = new Tourney { LiveScoreName = league },
                        MatchStatus = new MatchStatus { Name = matchStatus },
                        LiveMinute = liveMinute,
                        SeasonId = 1
                    });
                }
            }
            return matches;
        }
    }
}