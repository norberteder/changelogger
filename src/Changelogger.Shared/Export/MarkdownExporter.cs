using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Changelogger.Shared.Entity;

namespace Changelogger.Shared.Export
{
    class MarkdownExporter : ILogExporter
    {
        public string GetExportValue(IEnumerable<TicketDescriptor> logs, bool addLink, string versionFormat)
        {
            StringBuilder markdownBuilder = new StringBuilder();

            markdownBuilder.AppendLine("# Changelog");

            var grouped = logs.GroupBy(item => item.Version == null ? "master" : ReplaceVersion(versionFormat, item.Version));
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

        private string ReplaceVersion(string format, Version version)
        {
            if (!string.IsNullOrEmpty(format))
            {
                return format.ToLowerInvariant().Replace("{major}", version.Major.ToString()).Replace("{minor}", version.Minor.ToString()).Replace("{revision}", version.Revision.ToString()).Replace("{build}", version.Build.ToString());
            }
            return version.ToString();
        }
    }
}
