using DapperExtensions.Mapper;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Mappings
{
    class TestEntitieMapper : ClassMapper<TestEntitie>
    {
        public TestEntitieMapper()
        {
            base.Table("TestEntitie");
            Map(p => p.Id).Ignore();
            AutoMap();
        }
    }
}
