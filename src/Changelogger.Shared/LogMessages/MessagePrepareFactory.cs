namespace Changelogger.Shared.LogMessages
{
    public static class MessagePrepareFactory
    {
        public static MessagePreparer GetMessagePreparer()
        {
            return new PrepareMessagesDefault();
        }
    }
}
