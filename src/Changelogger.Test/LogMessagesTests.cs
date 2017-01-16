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
        string commit1 = "#1: Add initial code";
        string commit2 = "#2: Add an awesome feature";
        string commit3 = "#3: Update readme";
        string commit4 = "#4: Add hot new feature";
        string commit5 = "#5: Update readme";
        string commit6 = "#6: Bugfix xy";
        string commit7 = "#7: Add feature 0815";
        string commit8 = "#8: Remove feature 4711";
        string tag1 = "0.0.1";
        string tag2 = "0.0.2";
        string tag3 = "0.0.3";
        string tag4 = "0.0.4";

        [TestInitialize]
        public void Setup()
        {
            var hash1 = Guid.NewGuid().ToString();
            var hash2 = Guid.NewGuid().ToString();
            var hash3 = Guid.NewGuid().ToString();
            var hash4 = Guid.NewGuid().ToString();

            List<GitTag> tags = new List<GitTag>
            {
                new GitTag() { Hash = hash1, Name = tag1 },
                new GitTag() { Hash = hash2, Name = tag2 },
                new GitTag() { Hash = hash3, Name = tag3 },
                new GitTag() { Hash = hash4, Name = tag4 }
            };

            List<GitCommit> commits = new List<GitCommit>
            {
                new GitCommit() { Hash = hash4, CommitedAt = DateTime.Now, Message = commit8 },
                new GitCommit() { Hash = hash3, CommitedAt = DateTime.Now, Message = commit7 },
                new GitCommit() { Hash = Guid.NewGuid().ToString(), CommitedAt = DateTime.Now, Message = commit6 },
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

            Assert.AreEqual(commit1, logs[0].Message);
            Assert.AreEqual(commit2, logs[1].Message);
            Assert.AreEqual(commit3, logs[2].Message);
            Assert.AreEqual(commit4, logs[3].Message);
            Assert.AreEqual(commit5, logs[4].Message);
            Assert.AreEqual(commit6, logs[5].Message);
            Assert.AreEqual(commit7, logs[6].Message);
            Assert.AreEqual(commit8, logs[7].Message);

            Assert.AreEqual(tag1, logs[0].Tag);
            Assert.AreEqual(tag1, logs[1].Tag);
            Assert.AreEqual(tag1, logs[2].Tag);
            Assert.AreEqual(tag2, logs[3].Tag);
            Assert.AreEqual(tag2, logs[4].Tag);
            Assert.AreEqual(tag3, logs[5].Tag);
            Assert.AreEqual(tag3, logs[6].Tag);
            Assert.AreEqual(tag4, logs[7].Tag);
        }
    }
}
