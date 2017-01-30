using System.Collections.Generic;
using Changelogger.Shared.Entity;

namespace Changelogger.Shared.Ticketing
{
    public class DefaultTicketIntegration : ITicketingIntegrator
    {
        public IEnumerable<TicketDescriptor> CombineLogsWithTicketing(List<LogDescriptor> logs)
        {
            foreach (var log in logs)
            {
                yield return new TicketDescriptor() {Version = log.Tag, Title = log.Message};
            }
        }
    }
}
