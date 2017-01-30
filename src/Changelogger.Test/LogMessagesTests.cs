using Microsoft.VisualStudio.TestTools.UnitTesting;
using Changelogger.Shared.LogMessages;
using Changelogger.Git.Entity;
using System.Collections.Generic;
using System;
using System.Linq;
using Changelogger.Git;

namespace Changelogger.Test
{
    [TestClass]
    public class LogMessagesTests
    {
        GitRepository repo;
        string commit1 = "#1: Add initial code";
        string commit2 = "#2: Add an awesome feature";
        string commit3 = "#3: Update readme";
        string commit4 = "#4: Add hot new feature";
        string commit5 = "#5: Update readme";
        string tag1 = "0.0.1";
        string tag2 = "0.0.2";

        [TestInitialize]
        public void Setup()
        {
            var hash1 = Guid.NewGuid().ToString();
            var hash2 = Guid.NewGuid().ToString();

            List<GitTag> tags = new List<GitTag>
            {
                new GitTag() { Hash = hash1, Name = tag1 },
                new GitTag() { Hash = hash2, Name = tag2 }
            };

            List<GitCommit> commits = new List<GitCommit>
            {
                new GitCommit() { Hash = hash2, CommitedAt = DateTime.Now, Message = commit5 },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commit4 },
                new GitCommit() { Hash = hash1, CommitedAt = DateTime.Now, Message = commit3 },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commit2 },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commit1 }
            };

            repo = new GitRepository(tags, commits, GitSortStrategy.Time);
        }

        [TestMethod]
        public void Create_TimeMessagePreparer_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer();
            Assert.IsNotNull(timeMessagePreparer);
        }

        [TestMethod]
        public void Create_ReversedTimeMessagePreparer_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer();
            Assert.IsNotNull(timeMessagePreparer);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "gitInformation")]
        public void Prepare_LogMessagesWithoutRepo_ShouldThrowException()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer();
            var logs = timeMessagePreparer.PrepareMessages(null).ToList();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "Tags")]
        public void Prepare_LogMessagesWithoutTagsAndCommits_ShouldThrowException()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer();
            var gitInformation = new GitInformation(null, GitSortStrategy.Reverse | GitSortStrategy.Time);
            gitInformation.Repository = new GitRepository(null, null, GitSortStrategy.None);
            var logs = timeMessagePreparer.PrepareMessages(gitInformation).ToList();
        }

        [TestMethod]
        public void Prepare_LogMessagesWithEmptyTagsAndCommits_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer();
            var gitInformation = new GitInformation(null, GitSortStrategy.Reverse | GitSortStrategy.Time);
            gitInformation.Repository = new GitRepository(new List<GitTag>(), new List<GitCommit>(), GitSortStrategy.None);
            var logs = timeMessagePreparer.PrepareMessages(gitInformation).ToList();

            Assert.IsNotNull(logs);
            Assert.AreEqual(0, logs.Count);
        }
    }
}
