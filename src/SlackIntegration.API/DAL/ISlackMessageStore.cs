using SlackIntegration.SlackLibrary;
using System.Collections.Generic;

namespace SlackIntegration.DAL
{
    public interface ISlackMessageStore
    {
        SlackDbContext DbContext { get; set; }

        void SaveMessage(SlackMessage message);
        void SaveMessage(string text, string userName = null, string channel = null);
        List<SlackMessage> GetMessages();
        List<SlackMessage> GetLastMessages(int messagesCount);
    }
}
