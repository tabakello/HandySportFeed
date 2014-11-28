using System;
using HandySportFeed.Domain;
using HandySportFeed.Domain.Interfaces;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            var match = RepositoryFactory.GetRepository<IMatchRepository>().FindById(1);
            var mathesBySeason = RepositoryFactory.GetRepository<IMatchRepository>().GetMathesBySeason(1);

            foreach (var m in mathesBySeason)
            {
                Console.WriteLine(m.AwayCommand.NameRu);
            }
        }
    }
}
