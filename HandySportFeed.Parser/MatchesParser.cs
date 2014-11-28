using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Parsers
{
    public abstract class MatchesParser
    {
        private readonly IMatchesParserStrategy _parseStrategy;

        protected MatchesParser(IMatchesParserStrategy parseStrategy)
        {
            _parseStrategy = parseStrategy;
        }

        public IEnumerable<Match> Parse()
        {
            return _parseStrategy.Parse();
        }
    }
}
