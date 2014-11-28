using System;
using System.Collections.Generic;
using HandySportFeed.Parsers;
using HandySportFeed.Parsers.LiveScoreMachesParser;
using HandySportFeed.Parsers.ScoresProMatchesParser;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            //var match = RepositoryFactory.GetRepository<IMatchRepository>().FindById(1);
            //var mathesBySeason = RepositoryFactory.GetRepository<IMatchRepository>().GetMathesBySeason(1);

            //foreach (var m in mathesBySeason)
            //{
            //    Console.WriteLine(m.AwayCommand.NameRu);
            //}

            var parsers = new List<MatchesParser> {new ScoresProMatchesParser()};

            foreach (var matchesParser in parsers)
            {
                try
                {
                    var matches = matchesParser.Parse();
                    return;
                }
                catch (Exception e)
                {
                    var error = e.Message;
                    //Logger.Error(e)
                }
            }
        }
    }
}
