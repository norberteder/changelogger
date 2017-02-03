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

        public System.Version SpecificTag { get; set; }

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

        public IEnumerable<VersionTagRange> Tags
        {
            get
            {
                if (SpecificTag == null)
                {
                    var tags = Repository.Tags.OrderByDescending(item => item.Version);
                    var count = tags.Count();

                    for (int i = 0; i < count; i++)
                    {
                        var tagRange = new VersionTagRange();

                        if (i + 1 < count)
                        {
                            tagRange.VersionFrom = tags.ElementAt(i).Version;
                            tagRange.VersionTo = tags.ElementAt(i + 1).Version;
                        }
                        else
                        {
                            if (SpecificTag != null)
                            {
                                tagRange.VersionFrom = SpecificTag;
                                tagRange.VersionTo = null;
                            }
                            else
                            {
                                tagRange.VersionFrom = null;
                                tagRange.VersionTo = tags.ElementAt(i).Version;
                            }
                        }
                        yield return tagRange;
                    }
                }
                else
                {
                    yield return new VersionTagRange() { VersionFrom = SpecificTag, VersionTo = SpecificTag};
                }
            }
        }

        public IEnumerable<GitCommit> GetCommitsFromTagTo(System.Version versionFrom, System.Version versionTo)
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Reverse | CommitSortStrategies.Time,
                IncludeReachableFrom = versionFrom != null ? versionFrom.ToString() : "master", // sincelist
            };

            if (versionTo != null)
                filter.ExcludeReachableFrom = versionTo.ToString(); // untillist;

            using (var repo = new Repository(RepositoryPath))
            {
                var commits = repo.Commits.QueryBy(filter);
                foreach (var commit in commits)
                {
                    yield return new GitCommit() {Hash = commit.Sha, Message = commit.Message.Trim(), CommitedAt = commit.Committer.When};
                }
            }
        }

        private IEnumerable<GitVersionTag> GetTags()
        {
            using (var repo = new Repository(RepositoryPath))
            {
                foreach(var tag in repo.Tags)
                {
                    Trace.TraceInformation("Found Tag: {0} {1}", tag.Target.Sha, tag.FriendlyName);
                    System.Version tagVersion;
                    if (System.Version.TryParse(tag.FriendlyName, out tagVersion))
                    {
                        yield return new GitVersionTag {Hash = tag.Target.Sha, ReferenceHash = tag.Reference.TargetIdentifier, Version = tagVersion};
                    }
                    else
                    {
                        Trace.TraceWarning("Tag {0} is not a version tag and therefore not recognized", tag.FriendlyName);
                    }
                }
            }
        }

        private IEnumerable<GitCommit> GetCommits()
        {
            using(var repo = new Repository(RepositoryPath))
            {
                foreach(var commit in repo.Commits)
                {
                    Trace.TraceInformation("Found commit {0} {1} {2}", commit.Committer.When, commit.Sha, commit.Message.Trim());
                    yield return new GitCommit() { Hash = commit.Sha, Message = commit.Message.Trim(), CommitedAt = commit.Committer.When };
                }
            }
        }
    }
}
