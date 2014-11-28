using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> GetMathesBySeason(int seasonId);
    }
}
