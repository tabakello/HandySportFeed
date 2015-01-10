using System.Linq;
using Dapper;
using HandySportFeed.Domain.Interfaces;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Repositories {
    public class CommandRepository : AbstractDapperRepository<Command>, ICommandRepository {
        public CommandRepository() : base("Command", "TestDB") { }

        public override Command FindById(int id)
        {
            Command command;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"SELECT c.*, t.* FROM Command c
                                    Join Tourney t on c.TourneyId = t.Id
                                    where c.Id = @id";
                command = cn.Query<Command, Tourney, Command>(sql, (theCommand, tourney) => {
                    theCommand.Tourney = tourney;
                    return theCommand;
                }, new { id }).FirstOrDefault();
                cn.Close();
            }
            return command;
        }

        public Command FindByLiveScoreName(string name) {
            Command command;
            using (var cn = Connection)
            {
                cn.Open();
                const string sql = @"SELECT c.*, t.* FROM Command c
                                    Join Tourney t on c.TourneyId = t.Id
                                    where c.LiveScoreName = @name";
                command = cn.Query<Command, Tourney, Command>(sql, (theCommand, tourney) =>
                {
                    theCommand.Tourney = tourney;
                    return theCommand;
                }, new { name }).FirstOrDefault();
                cn.Close();
            }
            return command;
        }
    }
}