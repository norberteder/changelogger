using Microsoft.VisualStudio.TestTools.UnitTesting;
using Changelogger.Shared.LogMessages;
using Changelogger.Git.Entity;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Changelogger.Test
{
    [TestClass]
    public class LogMessagesTests
    {
        GitRepository repo;
        string[] commitMessages = new[]
        {
            "#1: Add initial code",
            "#2: Add an awesome feature",
            "#3: Update readme",
            "#4: Add hot new feature",
            "#5: Update readme",
            "#6: Bugfix xy",
            "#7: Add feature 0815",
            "#8: Remove feature 4711"
        };

        string[] commitTags = new[]
        {
            "0.0.1",
            "0.0.2",
            "0.0.3",
            "0.0.4"
        };

        [TestInitialize]
        public void Setup()
        {
            var hash1 = Guid.NewGuid().ToString();
            var hash2 = Guid.NewGuid().ToString();
            var hash3 = Guid.NewGuid().ToString();
            var hash4 = Guid.NewGuid().ToString();

            List<GitTag> tags = new List<GitTag>
            {
                new GitTag() { Hash = hash1, Name = commitTags[0] },
                new GitTag() { Hash = hash2, Name = commitTags[1] },
                new GitTag() { Hash = hash3, Name = commitTags[2] },
                new GitTag() { Hash = hash4, Name = commitTags[3] }
            };

            List<GitCommit> commits = new List<GitCommit>
            {
                new GitCommit() { Hash = hash4, CommitedAt = DateTime.Now, Message = commitMessages[7] },
                new GitCommit() { Hash = hash3, CommitedAt = DateTime.Now, Message = commitMessages[6] },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commitMessages[5] },
                new GitCommit() { Hash = hash2, CommitedAt = DateTime.Now, Message = commitMessages[4] },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commitMessages[3] },
                new GitCommit() { Hash = hash1, CommitedAt = DateTime.Now, Message = commitMessages[2] },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commitMessages[1] },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commitMessages[0] }
            };

            repo = new GitRepository(tags, commits, GitSortStrategy.Time);
        }

        [TestMethod]
        public void Create_TimeMessagePreparer_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            Assert.IsNotNull(timeMessagePreparer);
        }

        [TestMethod]
        public void Create_ReversedTimeMessagePreparer_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time | GitSortStrategy.Reverse);
            Assert.IsNotNull(timeMessagePreparer);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "repository")]
        public void Prepare_LogMessagesWithoutRepo_ShouldThrowException()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            var logs = timeMessagePreparer.PrepareMessages(null).ToList();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "Tags")]
        public void Prepare_LogMessagesWithoutTagsAndCommits_ShouldThrowException()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            var logs = timeMessagePreparer.PrepareMessages(new GitRepository(null, null, GitSortStrategy.None)).ToList();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "Commits")]
        public void Prepare_LogMessagesWithoutCommits_ShouldThrowException()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            var logs = timeMessagePreparer.PrepareMessages(new GitRepository(new List<GitTag>(), null, GitSortStrategy.None)).ToList();
        }

        [TestMethod]
        public void Prepare_LogMessagesWithEmptyTagsAndCommits_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            var logs = timeMessagePreparer.PrepareMessages(new GitRepository(new List<GitTag>(), new List<GitCommit>(), GitSortStrategy.None)).ToList();

            Assert.IsNotNull(logs);
            Assert.AreEqual(0, logs.Count);
        }

        [TestMethod]
        public void Prepare_LogMessagesByTime_ShouldBeOk()
        {
            var timeMessagePreparer = MessagePrepareFactory.GetMessagePreparer(GitSortStrategy.Time);
            var logs = timeMessagePreparer.PrepareMessages(repo).ToList();
            Assert.IsNotNull(logs);
            Assert.AreEqual<int>(8, logs.Count);

            for (int i = 0; i < commitMessages.Length; i++)
            {
                Assert.AreEqual(commitMessages[i], logs[i].Message);
            }

            Assert.AreEqual(commitTags[0], logs[0].Tag);
            Assert.AreEqual(commitTags[0], logs[1].Tag);
            Assert.AreEqual(commitTags[0], logs[2].Tag);
            Assert.AreEqual(commitTags[1], logs[3].Tag);
            Assert.AreEqual(commitTags[1], logs[4].Tag);
            Assert.AreEqual(commitTags[2], logs[5].Tag);
            Assert.AreEqual(commitTags[2], logs[6].Tag);
            Assert.AreEqual(commitTags[3], logs[7].Tag);
        }
    }
}
