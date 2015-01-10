using System.Collections.Generic;
using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class MatchRepository : AbstractDapperRepository<Match>, IMatchRepository {
        
        public MatchRepository()
            : base("Match", "TestDB") { }

        public IEnumerable<Match> GetMathesBySeason(int seasonId) {
            IEnumerable<Match> matches;
            using (var cn = Connection) {
                cn.Open();
                const string sql = @"select m.*, cH.*, cA.* from Match m  
                                     left join Command cH on m.HomeCommandId = cH.Id 
                                     left join Command cA on m.AwayCommandId = cA.Id
                                     where m.SeasonId = @seasonId";
                matches = cn.Query<Match, Command, Command, Match>(sql, (match, commandH, commandA) => {
                    match.HomeCommand = commandH;
                    match.AwayCommand = commandA;
                    return match;
                }, new { seasonId });
                cn.Close();
            }
            return matches;
        }

        public override Match FindById(int id) {
            Match match;
            using (var cn = Connection) {
                cn.Open();
                const string sql = @"select m.*, cH.*, cA.* from Match m  
                                     left join Command cH on m.HomeCommandId = cH.Id 
                                     left join Command cA on m.AwayCommandId = cA.Id 
                                     where m.Id = @id";

                match = cn.Query<Match, Command, Command, Match>(sql, (retval, commandH, commandA) => {
                    retval.AwayCommand = commandA;
                    retval.HomeCommand = commandH;
                    return retval;
                }, new { id }).First();

                cn.Close();
            }

            return match;
        }

        public IEnumerable<Match> GetMathesByLiveScoreCommandsName(string homeCommandName, string awayCommandName)
        {
            IEnumerable<Match> matches;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"select m.*, cH.*, cA.* from Match m 
                                     left join Command cH on m.HomeCommandId = cH.Id 
                                     left join Command cA on m.AwayCommandId = cA.Id
                                     where cH.LiveScoreName = @homeCommandName and cA.LiveScoreName = @awayCommandName";
                matches = cn.Query<Match, Command, Command, Match>(sql, (match, commandH, commandA) =>
                {
                    match.HomeCommand = commandH;
                    match.AwayCommand = commandA;
                    return match;
                }, new { homeCommandName, awayCommandName });
                cn.Close();
            }
            return matches;
        }
    }
}