using System;

namespace Changelogger.Git.Entity
{
    public class GitCommit
    {
        public string Hash { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CommitedAt { get; set; }
    }
}
