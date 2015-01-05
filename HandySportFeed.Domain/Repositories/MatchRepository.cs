using System.Collections.Generic;
using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class MatchRepository : AbstractDapperRepository<Match>, IMatchRepository {
        private const string CreateSql =
            @"create table #Matches (Id int, TourneyId int, Date datetime, HomeCommandId int, AwayCommandId int, ResultId int, ScoreHome int, ScoreAway int, SeasonId int) 
                                       create table #Commands (Id int, TourneyId int, NameEn varchar(59), NameRu varchar(55)) 
                                
                                       insert into #Matches values(1,1,getDate(),1,2,1,4,4,1)
                                       insert into #Matches values(1,1,getDate(),2,1,1,4,4,1)

                                       insert into #Commands values(1,1,'Sevastopol','Sevastopol')
                                       insert into #Commands values(2,1,'Simferopol','Simferopol')";

        public MatchRepository()
            : base("Match", "TestDB") { }

        public IEnumerable<Match> GetMathesBySeason(int seasonId) {
            IEnumerable<Match> matches;
            using (var cn = Connection) {
                cn.Open();
                cn.Execute(CreateSql);

                const string sql = @"select m.*, cH.*, cA.* from #Matches m  
                                     left join #Commands cH on m.HomeCommandId = cH.Id 
                                     left join #Commands cA on m.AwayCommandId = cA.Id
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
                cn.Execute(CreateSql);

                const string sql = @"select m.*, cH.*, cA.* from #Matches m  
                                     left join #Commands cH on m.HomeCommandId = cH.Id 
                                     left join #Commands cA on m.AwayCommandId = cA.Id 
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
    }
}