using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Parsers.LiveScoreMachesParser {
    public class LiveScoreMatchesParserStrategy : IMatchesParserStrategy {
        public IEnumerable<Match> Parse() {
            return new List<Match> {
                new Match { AwayCommand = new Command { Id = 1, NameEn = "LiveScore" } },
                new Match { AwayCommand = new Command { Id = 1, NameEn = "TestCommand" } }
            };
        }
    }
}