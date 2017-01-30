using CommandLine;

namespace Changelogger
{
    public class Options
    {
        [Option('r', "repository path", Required = true)]
        public string RepositoryPath { get; set; }

        [Option('e', "export file name", Required = true)]
        public string ExportFileName { get; set; }

        [Option('t', "ticketing tool support")]
        public string TicketingTool { get; set; }
        [Option('l', "link tickets")]
        public bool LinkTickets { get; set; }
        [Option('v', "verbose")]
        public bool Verbose { get; set; }
        [Option("tag")]
        public string Tag { get; set; }
    }
}
