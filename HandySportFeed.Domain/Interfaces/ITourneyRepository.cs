using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Interfaces
{
    public interface ITourneyRepository : IRepository<Tourney> {
        Tourney FindByLiveScoreName(string name);
    }
}
