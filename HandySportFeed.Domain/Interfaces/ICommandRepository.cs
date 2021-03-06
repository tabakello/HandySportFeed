﻿using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Interfaces {
    public interface ICommandRepository : IRepository<Command> {
        Command FindByLiveScoreName(string name);
    }


}