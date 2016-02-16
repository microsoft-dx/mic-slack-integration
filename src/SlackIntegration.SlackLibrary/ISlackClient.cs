using System;

namespace SlackIntegration.SlackLibrary
{
    public interface ISlackClient
    {
        Uri Uri { get; }

        void PostMessage(string text, string userName = null, string channel = null);
        void PostMessage(SlackMessage message);
    }
}
