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

            Parser.Default.ParseArguments<Options>(args).WithParsed((options) =>
            {
                var program = new Program();
                program.HandleOptions(options);
                program.Start(options);
            });

            sw.Stop();

            Console.WriteLine("Created changelog in {0}", sw.Elapsed);

            Console.WriteLine("Done. Press any key.");
            Console.ReadKey();
        }

        private void HandleOptions(Options options)
        {
            if (options.Verbose)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }
            else
            {
                Trace.Listeners.Clear();
            }
        }

        private void Start(Options options)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            GitInformation info = new GitInformation();
            var repository = info.GetRepositoryInformation(options.RepositoryPath);

            watch.Stop();
            Trace.TraceInformation("GitInformation.GetRepositoryInformation took {0}ms", watch.ElapsedMilliseconds);

            watch.Restart();

            var preparer = MessagePrepareFactory.GetMessagePreparer(repository.SortStrategy);
            var logs = preparer.PrepareMessages(repository);

            watch.Stop();
            Trace.TraceInformation("MessagePreparer.PrepareMessages took {0}ms", watch.ElapsedMilliseconds);

            watch.Restart();

            var exporter = LogExporterFactory.GetLogExporter("markdown");
            var val = exporter.GetExportValue(logs);

            watch.Stop();
            Trace.TraceInformation("LogExporter.GetExportValue took {0}ms", watch.ElapsedMilliseconds);

            File.WriteAllText(options.ExportFileName, val);
        }
    }
}
