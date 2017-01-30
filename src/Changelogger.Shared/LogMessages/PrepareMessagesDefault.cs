using Changelogger.Shared.Entity;
using System.Collections.Generic;
using System.Diagnostics;
using Changelogger.Git;
using Changelogger.Shared.Helper;

namespace Changelogger.Shared.LogMessages
{
    class PrepareMessagesDefault : MessagePreparer
    {
        public bool Reverse { get; set; }

        public IEnumerable<LogDescriptor> PrepareMessages(GitInformation gitInformation)
        {
            Guard.ArgumentNotNull(gitInformation, nameof(gitInformation));

            var tagRanges = gitInformation.Tags;
            foreach(var range in tagRanges)
            {                
                var commits = gitInformation.GetCommitsFromTagTo(range.TagFrom, range.FromToAreEqual ? null : range.TagTo);
                foreach (var commit in commits)
                {
                    Trace.TraceInformation("Add commit {0} to tag {1}", commit.Message, range.TagFrom);
                    yield return new LogDescriptor() {Hash = commit.Hash, Message = commit.Message, Tag = range.TagFrom };
                }
            }
        }
    }
}
