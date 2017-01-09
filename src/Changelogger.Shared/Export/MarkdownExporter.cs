using System.Collections.Generic;
using System.Linq;
using System.Text;
using Changelogger.Shared.Entity;

namespace Changelogger.Shared.Export
{
    class MarkdownExporter : ILogExporter
    {
        public string GetExportValue(IEnumerable<LogDescriptor> logs)
        {
            StringBuilder markdownBuilder = new StringBuilder();

            markdownBuilder.AppendLine("# Changelog");

            var grouped = logs.GroupBy(item => item.Tag);
            foreach(IGrouping<string, LogDescriptor> log in grouped)
            {
                markdownBuilder.AppendFormat("## {0}", log.Key);
                markdownBuilder.AppendLine();

                foreach(var val in log)
                {
                    markdownBuilder.AppendLine("* " + val.Message.Replace("#", "\\#"));
                }
                markdownBuilder.AppendLine();
            }

            return markdownBuilder.ToString();
        }
    }
}
