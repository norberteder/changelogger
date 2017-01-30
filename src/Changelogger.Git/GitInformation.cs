using Changelogger.Git.Entity;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Changelogger.Git
{
    public class GitInformation
    {
        private string RepositoryPath { get; set; }
        private GitSortStrategy SortStrategy { get; set; }
        public GitRepository Repository { get; set; }

        public string SpecificTag { get; set; }

        public GitInformation(string repositoryPath, GitSortStrategy sortStrategy)
        {
            SortStrategy = sortStrategy;
            RepositoryPath = repositoryPath;
        }

        public GitRepository GetRepositoryInformation()
        {
            Repository = GetRepositoryInformation(RepositoryPath);
            return Repository;
        }

        public GitRepository GetRepositoryInformation(string repositoryPath)
        {
            RepositoryPath = repositoryPath;

            var tags = GetTags().ToList();
            var commits = GetCommits().ToList();

            
            return new GitRepository(tags, commits, SortStrategy);
        }

        // TODO return tag-range
        public IEnumerable<TagRange> Tags
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SpecificTag))
                {
                    var tags = Repository.Tags.OrderByDescending(item => item.Name);
                    var count = tags.Count();

                    for (int i = 0; i < count; i++)
                    {
                        var tagRange = new TagRange();

                        if (i + 1 < count)
                        {
                            tagRange.TagFrom = tags.ElementAt(i).Name;
                            tagRange.TagTo = tags.ElementAt(i + 1).Name;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(SpecificTag))
                            {
                                tagRange.TagFrom = SpecificTag;
                                tagRange.TagTo = "";
                            }
                            else
                            {
                                tagRange.TagFrom = "master";
                                tagRange.TagTo = tags.ElementAt(i).Name;
                            }
                        }
                        yield return tagRange;
                    }
                }
                else
                {
                    yield return new TagRange() {TagFrom = SpecificTag, TagTo = SpecificTag};
                }
            }
        }

        public IEnumerable<GitCommit> GetCommitsFromTagTo(string tag1, string tag2)
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Reverse | CommitSortStrategies.Time,
                IncludeReachableFrom = tag1, // sincelist
            };

            if (!string.IsNullOrWhiteSpace(tag2))
                filter.ExcludeReachableFrom = tag2; // untillist;

            using (var repo = new Repository(RepositoryPath))
            {
                var commits = repo.Commits.QueryBy(filter);
                foreach (var commit in commits)
                {
                    yield return new GitCommit() {Hash = commit.Sha, Message = commit.Message, CommitedAt = commit.Committer.When};
                }
            }
        }

        private IEnumerable<GitTag> GetTags()
        {
            using (var repo = new Repository(RepositoryPath))
            {
                foreach(var tag in repo.Tags)
                {
                    Trace.TraceInformation("Found Tag: {0} {1}", tag.Target.Sha, tag.FriendlyName);
                    yield return new GitTag { Hash = tag.Target.Sha, ReferenceHash = tag.Reference.TargetIdentifier, Name = tag.FriendlyName };
                }
            }
        }

        private IEnumerable<GitCommit> GetCommits()
        {
            using(var repo = new Repository(RepositoryPath))
            {
                foreach(var commit in repo.Commits)
                {
                    Trace.TraceInformation("Found commit {0} {1} {2}", commit.Committer.When, commit.Sha, commit.Message);
                    yield return new GitCommit() { Hash = commit.Sha, Message = commit.Message, CommitedAt = commit.Committer.When };
                }
            }
        }
    }
}
