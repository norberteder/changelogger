using Changelogger.Shared.Entity;
using System.Collections.Generic;

namespace Changelogger.Shared.Export
{
    public interface ILogExporter
    {
        string GetExportValue(IEnumerable<TicketDescriptor> logs, bool addLink);
    }
}
