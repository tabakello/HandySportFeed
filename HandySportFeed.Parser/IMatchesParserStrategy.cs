using System;
using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Parsers {
    public interface IMatchesParserStrategy {
        IEnumerable<Match> Parse(string url);
        IEnumerable<Match> GetTodayMatches(string url);
        IEnumerable<Match> GetMatchesByDate(string url, DateTime date);
    }
}