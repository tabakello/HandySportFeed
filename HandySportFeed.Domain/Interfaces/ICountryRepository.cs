
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Interfaces
{
    public interface ICountryRepository : IRepository<Country> {
        Country FindByName(string name);
    }
}
