using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class CommandRepository : AbstractDapperRepository<Command>, ICommandRepository {
        public CommandRepository() : base("Command", "TestDB") { }
    }
}