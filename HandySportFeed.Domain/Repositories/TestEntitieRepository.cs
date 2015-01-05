using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class TestEntitieRepository : AbstractDapperRepository<TestEntitie>, ITestEntitieRepository {
        public TestEntitieRepository()
            : base("TestEntitie", "TestDB") { }
    }
}