namespace HandySportFeed.Domain.Model {
    public class Command : EntityBase {
        public int TourneyId { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public int? Rate { get; set; }
        public bool Actual { get; set; }
        public string LogoUrl { get; set; }
        public string ScoreProName { get; set; }
        public string LiveScoreName { get; set; }
        public Tourney Tourney { get; set; }
    }
}