using Changelogger.Git;
using Changelogger.Shared.Export;
using Changelogger.Shared.LogMessages;
using CommandLine;
using System;
using System.IO;

namespace Changelogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(HandleOptions);

            Console.ReadKey();
        }

        private static void HandleOptions(Options options)
        {
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
