using Changelogger.Git.Entity;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Changelogger.Git
{
    public class GitInformation
    {
        public GitRepository GetRepositoryInformation(string repositoryPath)
        {
            using (var repository = new Repository(repositoryPath))
            {
                var tags = GetTags(repository).ToList();
                var commits = GetCommits(repository).ToList();
                var sortStrategy = GetSortStrategy(repository);

                return new GitRepository(tags, commits, sortStrategy);
            }
        }

        private List<GitTag> GetTags(Repository repository)
        {
            List<GitTag> tags = new List<GitTag>();
            foreach (var tag in repository.Tags)
            {
                Trace.TraceInformation("Found Tag: {0} {1}", tag.Reference.TargetIdentifier, tag.FriendlyName);
                tags.Add(new GitTag { Hash = tag.Reference.TargetIdentifier, Name = tag.FriendlyName });
            }
            return tags;
        }

        private List<GitCommit> GetCommits(Repository repository)
        {
            List<GitCommit> commits = new List<GitCommit>();
            foreach (var commit in repository.Commits)
            {
                Trace.TraceInformation("Found commit {0} {1} {2}", commit.Committer.When, commit.Sha, commit.Message);
                commits.Add(new GitCommit() { Hash = commit.Sha, Message = commit.Message, CommitedAt = commit.Committer.When });
            }
            return commits;
        }

        private GitSortStrategy GetSortStrategy(Repository repository)
        {
            Trace.TraceInformation("Repository commit sort strategy is " + repository.Commits.SortedBy.ToString());
            return (GitSortStrategy)repository.Commits.SortedBy;
        }
    }
}
