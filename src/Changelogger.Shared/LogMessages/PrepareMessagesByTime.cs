using Changelogger.Git.Entity;
using Changelogger.Shared.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Changelogger.Shared.Helper;

namespace Changelogger.Shared.LogMessages
{
    class PrepareMessagesByTime : MessagePreparer
    {
        public bool Reverse { get; set; }

        public IEnumerable<LogDescriptor> PrepareMessages(GitRepository repository)
        {
            Guard.ArgumentNotNull(repository, nameof(repository));
            Guard.ArgumentNotNull(repository.Tags, nameof(repository.Tags));
            Guard.ArgumentNotNull(repository.Commits, nameof(repository.Commits));

            if (!Reverse)
                repository.Commits.Reverse();

            List<GitCommit> commitList = new List<GitCommit>();
            foreach(var commit in repository.Commits)
            {
                commitList.Add(commit);

                if (repository.Tags.Any(item => item.Hash == commit.Hash))
                {
                    var tag = repository.Tags.First(item => item.Hash == commit.Hash);
                    // found tag
                    foreach(var item in commitList)
                    {
                        Trace.TraceInformation("Add to changelog {0} {1} {2}", tag.Name, item.Hash, item.Message);
                        yield return new LogDescriptor() { Hash = item.Hash, Message = item.Message, Tag = tag.Name };
                    }

                    commitList.Clear();
                }
            }
        }
    }
}
