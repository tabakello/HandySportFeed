using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Repositories;
using Ninject.Modules;

namespace HandySportFeed.Domain {
    public class RepositoryNinjectModule : NinjectModule {
        public override void Load() {
            Bind<ITestEntitieRepository>().To<TestEntitieRepository>();
            Bind<ICommandRepository>().To<CommandRepository>();
            Bind<IMatchRepository>().To<MatchRepository>();
        }
    }
}