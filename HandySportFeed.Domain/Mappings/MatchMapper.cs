using DapperExtensions.Mapper;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Mappings {
    public sealed class MatchMapper : ClassMapper<Match> {
        public MatchMapper() {
            Table("Match");
            Map(f => f.AwayCommand).Ignore();
            Map(f => f.HomeCommand).Ignore();
            Map(f => f.Country).Ignore();
            Map(f => f.Tourney).Ignore();
            AutoMap();
        }
    }
}