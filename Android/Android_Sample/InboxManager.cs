using AccengageSDK;

namespace Android_Sample
{
    public static class InboxManager
    {
        public static AccengageInbox Inbox { get; set; }

        public static AccenageInboxMessage CurrentMessage { get; set; }

        public static AccengageInboxMessageContent CurrentMessageContent { get; set; }
    }
}