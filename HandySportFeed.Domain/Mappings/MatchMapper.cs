using DapperExtensions.Mapper;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Mappings {
    public sealed class MatchMapper : ClassMapper<Match> {
        public MatchMapper() {
            Table("Match");
            AutoMap();
        }
    }
}