namespace HandySportFeed.Domain.Model {
    public class Tourney : EntityBase {
        public string Name { get; set; }
        public string ScoreProName { get; set; }
        public string LiveScoreName { get; set; }
    }
}