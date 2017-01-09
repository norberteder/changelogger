using Changelogger.Git.Entity;

namespace Changelogger.Shared.LogMessages
{
    public static class MessagePrepareFactory
    {
        public static MessagePreparer GetMessagePreparer(GitSortStrategy gitSortStrategy)
        {
            switch(gitSortStrategy)
            {
                case GitSortStrategy.Time:
                    return new PrepareMessagesByTime();
                case GitSortStrategy.Time | GitSortStrategy.Reverse:
                    return new PrepareMessagesByTime() { Reverse = true };
            }

            return null;
        }
    }
}
