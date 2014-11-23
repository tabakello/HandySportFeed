using Ninject;

namespace HandySportFeed.Domain
{
    public class RepositoryFactory
    {
        public static TIRepository GetRepository<TIRepository>()
        {
            var kernel = new StandardKernel(new RepositoryNinjectModule());
            return kernel.Get<TIRepository>();
        }
    }
}
