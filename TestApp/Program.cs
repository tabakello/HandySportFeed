using HandySportFeed.Domain;
using HandySportFeed.Domain.Interfaces;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            var a = RepositoryFactory.GetRepository<ITestEntitieRepository>().FindAll();
        }
    }
}
