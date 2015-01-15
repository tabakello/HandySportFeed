using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class MatchRepository : AbstractDapperRepository<Match>, IMatchRepository {
        
        public MatchRepository()
            : base("Match", "TestDB") { }

        public List<Match> GetMathesBySeason(int seasonId) {
            List<Match> matches;
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
                }, new { seasonId }).ToList();
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

        public List<Match> GetMathesByLiveScoreCommandsName(string homeCommandName, string awayCommandName)
        {
            List<Match> matches;
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
                }, new { homeCommandName, awayCommandName }).ToList();
                cn.Close();
            }
            return matches;
        }


        public List<Match> GetMatchesByDate(DateTime date)
        {
            List<Match> tourney;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"SELECT * FROM Match where date >= @startDate and date <= @endDate";
                tourney = cn.Query<Match>(sql, new {
                    startDate = date.Date + new TimeSpan(0, 0, 0),
                    endDate = date.Date + new TimeSpan(23, 59, 59)
                }).ToList();
                cn.Close();
            }
            return tourney;
        }
    }
}