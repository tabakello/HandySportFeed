using DapperExtensions.Mapper;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Mappings {
    internal sealed class TestEntitieMapper : ClassMapper<TestEntitie> {
        public TestEntitieMapper() {
            Table("TestEntitie");
            Map(p => p.Id).Ignore();
            AutoMap();
        }
    }
}