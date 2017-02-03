using System;
using Changelogger.Git;
using Changelogger.Shared.Export;
using Changelogger.Shared.LogMessages;
using CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Changelogger.Git.Entity;
using Changelogger.Shared.Ticketing;

namespace Changelogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(HandleOptions);
        }

        private static void HandleOptions(Options options)
        {
            if (!options.Verbose)
            {
                Trace.Listeners.Clear();
            }

            GitInformation info = new GitInformation(options.RepositoryPath, GitSortStrategy.Reverse | GitSortStrategy.Time);
            if (!string.IsNullOrEmpty(options.Tag))
            {
                Version specificVersion;
                if (Version.TryParse(options.Tag, out specificVersion))
                    info.SpecificTag = specificVersion;
            }

            info.GetRepositoryInformation();

            var preparer = MessagePrepareFactory.GetMessagePreparer();
            var logs = preparer.PrepareMessages(info).ToList();

            var ticketIntegrator = TicketingFactory.GetTicketingIntegrator(options.TicketingTool);
            var tickets = ticketIntegrator.CombineLogsWithTicketing(logs);

            var exporter = LogExporterFactory.GetLogExporter("markdown");
            var val = exporter.GetExportValue(tickets, options.LinkTickets);

            File.WriteAllText(options.ExportFileName, val);
        }
    }
}
