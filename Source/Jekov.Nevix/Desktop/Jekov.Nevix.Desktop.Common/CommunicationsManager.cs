namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Desktop.Common.Contracts;
    using PubNubMessaging.Core;
    using System;

    public class CommunicationsManager
    {
        private const string PublishKey = "pub-c-2db685fb-40f0-4f91-a074-31ab9993d2d6";
        private const string SubscribeKey = "sub-c-c7a22dee-6f0c-11e3-9291-02ee2ddab7fe";
        private const string SecretKey = "sec-c-Y2E2YzNjMTUtZjhiMi00ZDFiLTg0OTEtZmIxMTg4NGEwYzk4";

        public string ChannelName { get; protected set; }

        public CommunicationsManager(string channelName, CommandExecutor executor)
        {
            this.executor = executor;
            this.ChannelName = channelName;
            push = new Pubnub(PublishKey, SubscribeKey, SecretKey);
            push.Subscribe<string>(this.ChannelName, HandleIncomingData, HandleConnection, HandleError);
        }

        private void HandleError(string data)
        {
            Console.WriteLine(data);
        }

        private readonly Pubnub push;
        private CommandExecutor executor;

        private void HandleIncomingData(string data)
        {
            string cmd = data.TrimStart('[').Substring(0, data.IndexOf(',') - 1).Trim('\\').Trim('"');
            Console.WriteLine(cmd);
            executor.ExecuteCommand(cmd);
        }

        private void HandleConnection(string data)
        {
            Console.WriteLine(data);
        }

        private void Call(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
        }
    }
}