using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Interfaces {
    public interface IMatchRepository : IRepository<Match> {
        List<Match> GetMathesBySeason(int seasonId);
    }
}