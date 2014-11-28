using System.Linq;
using Dapper;
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
            
            const string createSql = @"create table #Matches (Id int, TourneyId int, Date datetime, HomeCommandId int, AwayCommandId int, ResultId int, ScoreHome int, ScoreAway int, SeasonId int) 
                              create table #Commands (Id int, TourneyId int, NameEn varchar(59), NameRu varchar(55)) 
                                
                                insert into #Matches values(1,1,getDate(),1,2,1,4,4,1)
                                insert into #Commands values(1,1,'Sevastopol','Sevastopol')
                                insert into #Commands values(2,1,'Simferopol','Simferopol')";
            Match match;
            using (var cn = Connection)
            {
                cn.Open();
                cn.Execute(createSql);

                const string sql = @"select m.*, cH.*, cA.* from #Matches m  
                                     left join #Commands cH on m.HomeCommandId = cH.Id 
                                     left join #Commands cA on m.AwayCommandId = cA.Id ";

                match = cn.Query<Match, Command, Command, Match>(sql, (retval, commandH, commandA) =>
                {
                    retval.AwayCommand = commandA;
                    retval.HomeCommand = commandH;
                    return retval;
                }).First();
                
                cn.Close();
            }

            return match;
        }
    }
}
