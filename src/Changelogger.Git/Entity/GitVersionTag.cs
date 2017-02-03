using System;

namespace Changelogger.Git.Entity
{
    public class GitVersionTag
    {
        public string Hash { get; set; }
        public string ReferenceHash { get; set; }
        public Version Version { get; set; }
    }
}
