using System.Collections.Generic;
using Changelogger.Shared.Entity;

namespace Changelogger.Shared.Ticketing
{
    public interface ITicketingIntegrator
    {
        IEnumerable<TicketDescriptor> CombineLogsWithTicketing(List<LogDescriptor> logs);
    }
}
