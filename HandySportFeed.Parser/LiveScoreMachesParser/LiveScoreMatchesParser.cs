namespace HandySportFeed.Parsers.LiveScoreMachesParser
{
    public class LiveScoreMatchesParser : MatchesParser
    {
        public LiveScoreMatchesParser() : base(new LiveScoreMatchesParserStrategy()) { }
    }
}
