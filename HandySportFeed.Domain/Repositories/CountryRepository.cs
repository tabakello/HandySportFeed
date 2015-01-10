using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories
{
    public class CountryRepository : AbstractDapperRepository<Country>, ICountryRepository {
        public CountryRepository() : base("Country", "TestDb") { }

        public Country FindByName(string name) {
            Country country;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"SELECT * FROM Country where Name = @name";
                country = cn.Query<Country>(sql, new { name }).FirstOrDefault();
                cn.Close();
            }
            return country;
        }
    }
}
