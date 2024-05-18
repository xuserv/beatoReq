using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using TwitchLib.Communication.Clients;

namespace beatoReq {
    class TwitchBot {
        private TwitchClient twitchClient;
        private string _channelName = string.Empty;
        private string _accessToken = string.Empty;
        
        public TwitchBot(string channelName, string accessToken) {
            _channelName = channelName;
            _accessToken = accessToken;

            ConnectionCredentials credentials = new ConnectionCredentials(_channelName, _accessToken);
            WebSocketClient wsClient = new WebSocketClient();
            twitchClient = new TwitchClient(wsClient);
            twitchClient.Initialize(credentials, _channelName);
            twitchClient.OnConnected += Client_OnConnected;
            twitchClient.OnMessageReceived += Client_OnMessageReceived;
#if VERBOSE
            twitchClient.OnLog += Client_OnLog;
#endif
            twitchClient.Connect();

            Program.pipeWriter.WriteLine("START");
        }

#if VERBOSE
        private void Client_OnLog(object? sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }
#endif
  
        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {_channelName}");
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime()} - [{e.ChatMessage.DisplayName}] {e.ChatMessage.Message}");
            if (!Program.beatorajaPipe.IsConnected) return;

            if (e.ChatMessage.Message.StartsWith("!!req ")) {
                Program.pipeWriter.WriteLine(e.ChatMessage.Message);
            }
        }
    }
}
