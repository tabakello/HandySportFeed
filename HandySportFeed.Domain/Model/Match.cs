namespace HandySportFeed.Domain.Model {
    public class Match : EntityBase {
        public int TourneyId { get; set; }
        public System.DateTime Date { get; set; }
        public string Tour { get; set; }
        public int HomeCommandId { get; set; }
        public int AwayCommandId { get; set; }
        public int ScoreHome { get; set; }
        public int ScoreAway { get; set; }
        public int SeasonId { get; set; }
        public int ResultId { get; set; }
        public int MatchStatusId { get; set; }
        public string LiveMinute { get; set; }

        public Command HomeCommand { get; set; }
        public Command AwayCommand { get; set; }
        public Tourney Tourney { get; set; }
        public Country Country { get; set; }
        public MatchStatus MatchStatus { get; set; }
    }
}