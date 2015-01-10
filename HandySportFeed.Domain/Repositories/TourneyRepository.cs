using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories
{
    public class TourneyRepository : AbstractDapperRepository<Tourney>, ITourneyRepository {
        public TourneyRepository() : base("Tourney", "TestDB") { }

        public Tourney FindByLiveScoreName(string name) {
            Tourney tourney;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"SELECT * FROM Tourney where LiveScoreName = @name";
                tourney = cn.Query<Tourney>(sql, new { name }).FirstOrDefault();
                cn.Close();
            }
            return tourney;
        }
    }
}
