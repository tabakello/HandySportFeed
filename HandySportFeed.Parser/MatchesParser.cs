using System;
using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Parsers {
    public class MatchesParser {
        private readonly IMatchesParserStrategy parseStrategy;

        public MatchesParser(IMatchesParserStrategy parseStrategy) {
            this.parseStrategy = parseStrategy;
        }

        public IEnumerable<Match> Parse(string url, DateTime date) {
            return parseStrategy.Parse(url, date);
        }
    }
}