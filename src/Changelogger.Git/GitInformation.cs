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
            var tags = GetTags(repositoryPath).ToList();
            var commits = GetCommits(repositoryPath).ToList();
            var sortStrategy = GetSortStrategy(repositoryPath);

            return new GitRepository(tags, commits, sortStrategy);
        }

        private IEnumerable<GitTag> GetTags(string repositoryPath)
        {
            using (var repo = new Repository(repositoryPath))
            {
                foreach(var tag in repo.Tags)
                {
                    Trace.TraceInformation("Found Tag: {0} {1}", tag.Reference.TargetIdentifier, tag.FriendlyName);
                    yield return new GitTag { Hash = tag.Reference.TargetIdentifier, Name = tag.FriendlyName };
                }
            }
        }

        private IEnumerable<GitCommit> GetCommits(string repositoryPath)
        {
            using(var repo = new Repository(repositoryPath))
            {
                foreach(var commit in repo.Commits)
                {
                    Trace.TraceInformation("Found commit {0} {1} {2}", commit.Committer.When, commit.Sha, commit.Message);
                    yield return new GitCommit() { Hash = commit.Sha, Message = commit.Message, CommitedAt = commit.Committer.When };
                }
            }
        }

        private GitSortStrategy GetSortStrategy(string repositoryPath)
        {
            using (var repo = new Repository(repositoryPath))
            {
                //var repoSortVal = repo.Commits.SortedBy.ToString();
                //GitSortStrategy strategy = (GitSortStrategy)Enum.Parse(typeof(GitSortStrategy), repoSortVal);
                Trace.TraceInformation("Repository commit sort strategy is " + repo.Commits.SortedBy.ToString());
                return (GitSortStrategy)repo.Commits.SortedBy;
            }
        }
    }
}
