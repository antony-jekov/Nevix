namespace Jekov.Nevix.Desktop.Common
{
    using PubNubMessaging.Core;
    using System;

    public class CommunicationsManager
    {
        private const string PublishKey = "pub-c-2db685fb-40f0-4f91-a074-31ab9993d2d6";
        private const string SubscribeKey = "sub-c-c7a22dee-6f0c-11e3-9291-02ee2ddab7fe";
        private const string SecretKey = "sec-c-Y2E2YzNjMTUtZjhiMi00ZDFiLTg0OTEtZmIxMTg4NGEwYzk4";

        public string ChannelName { get; protected set; }

        public CommunicationsManager(string channelName)
        {
            this.ChannelName = channelName;
            push = new Pubnub(PublishKey, SubscribeKey, SecretKey);
            player = new PlayerManager();
            push.Subscribe<string>(this.ChannelName, HandleIncomingData, HandleConnection, HandleError);
        }

        private void HandleError(string data)
        {
            Console.WriteLine(data);
        }

        private readonly Pubnub push;
        private readonly PlayerManager player;

        private void HandleIncomingData(string data)
        {
            string cmd = data.TrimStart('[').Substring(0, data.IndexOf(',') - 1).Trim('\\').Trim('"');

            player.ExecuteCmd(cmd);
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