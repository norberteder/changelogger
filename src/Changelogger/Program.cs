using Changelogger.Git;
using Changelogger.Shared.Export;
using Changelogger.Shared.LogMessages;
using CommandLine;
using System;
using System.Diagnostics;
using System.IO;

namespace Changelogger
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            Parser.Default.ParseArguments<Options>(args).WithParsed(HandleOptions);

            sw.Stop();

            Trace.TraceInformation("Created changelog in {0}", sw.Elapsed);

            Console.WriteLine("Done. Press any key.");
            Console.ReadKey();
        }

        private static void HandleOptions(Options options)
        {
            if (options.Verbose)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }

            GitInformation info = new GitInformation();
            var repository = info.GetRepositoryInformation(options.RepositoryPath);

            var preparer = MessagePrepareFactory.GetMessagePreparer(repository.SortStrategy);
            var logs = preparer.PrepareMessages(repository);
            
            var exporter = LogExporterFactory.GetLogExporter("markdown");
            var val = exporter.GetExportValue(logs);

            File.WriteAllText(options.ExportFileName, val);
        }
    }
}
