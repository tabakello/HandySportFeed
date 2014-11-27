using DapperExtensions.Mapper;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain.Mappings
{
    public sealed class CommandMapper : ClassMapper<Command>
    {
        public CommandMapper()
        {
            Table("Command");
            AutoMap();
        }
    }
}
