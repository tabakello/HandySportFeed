using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Parsers {
    public abstract class MatchesParser {
        private readonly IMatchesParserStrategy parseStrategy;

        protected MatchesParser(IMatchesParserStrategy parseStrategy) {
            this.parseStrategy = parseStrategy;
        }

        public IEnumerable<Match> Parse() {
            return parseStrategy.Parse();
        }
    }
}