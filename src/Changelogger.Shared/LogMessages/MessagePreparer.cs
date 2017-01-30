using Changelogger.Git.Entity;
using Changelogger.Shared.Entity;
using System.Collections.Generic;
using Changelogger.Git;

namespace Changelogger.Shared.LogMessages
{
    public interface MessagePreparer
    {
        bool Reverse { get; set; }
        IEnumerable<LogDescriptor> PrepareMessages(GitInformation gitInformation);
    }
}
