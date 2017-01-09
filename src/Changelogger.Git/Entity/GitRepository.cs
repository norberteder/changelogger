using System.Collections.Generic;

namespace Changelogger.Git.Entity
{
    public class GitRepository
    {
        public List<GitTag> Tags { get; private set; }
        public List<GitCommit> Commits { get; private set; }
        public GitSortStrategy SortStrategy { get; private set; }

        public GitRepository(List<GitTag> tags, List<GitCommit> commits, GitSortStrategy sortStrategy)
        {
            Tags = tags;
            Commits = commits;
            SortStrategy = sortStrategy;
        }
    }
}
