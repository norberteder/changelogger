using CommandLine;

namespace Changelogger
{
    public class Options
    {
        [Option('r', "repository path", Required = true)]
        public string RepositoryPath { get; set; }

        [Option('e', "export file name", Required = true)]
        public string ExportFileName { get; set; }

        [Option('v', "verbose")]
        public bool Verbose { get; set; }
    }
}
