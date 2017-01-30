using System.Collections.Generic;
using System.Linq;
using System.Text;
using Changelogger.Shared.Entity;

namespace Changelogger.Shared.Export
{
    class MarkdownExporter : ILogExporter
    {
        public string GetExportValue(IEnumerable<TicketDescriptor> logs, bool addLink)
        {
            StringBuilder markdownBuilder = new StringBuilder();

            markdownBuilder.AppendLine("# Changelog");

            var grouped = logs.GroupBy(item => item.Version);
            foreach(IGrouping<string, TicketDescriptor> log in grouped)
            {
                markdownBuilder.AppendFormat("## {0}", log.Key);
                markdownBuilder.AppendLine();

                foreach(var val in log)
                {
                    if (addLink)
                    {
                        markdownBuilder.AppendLine("* [" + val.Title + "](" + val.Link + " \"" + val.Title + "\")");
                    }
                    else
                        markdownBuilder.AppendLine("* " + val.Title);
                }
                markdownBuilder.AppendLine();
            }

            return markdownBuilder.ToString();
        }
    }
}
