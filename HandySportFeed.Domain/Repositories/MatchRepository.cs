using DapperExtensions;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories
{
    public class MatchRepository : AbstractDapperRepository<Match>, IMatchRepository
    {
        public MatchRepository()
            : base("Match", "TestDB")
        {

        }

        public override Match FindById(int id)
        {
            Match match;
            using (var cn = Connection)
            {
                cn.Open();
                match = cn.Get<Match>(id);
                if (match != null)
                {
                    match.HomeCommand = cn.Get<Command>(match.HomeCommandId);
                    match.AwayCommand = cn.Get<Command>(match.AwayCommandId);
                }
                cn.Close();
            }
            
            return match;
        }
    }
}
