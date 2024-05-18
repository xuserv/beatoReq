using System.IO.Pipes;

namespace beatoReq {
    class Program {
        public static NamedPipeServerStream beatorajaPipe = new NamedPipeServerStream("beatoraja", PipeDirection.Out);
        public static StreamWriter pipeWriter = new StreamWriter(beatorajaPipe);

        static void Main() {
            string channelName = "";
            string accessToken = "";
            if (string.IsNullOrEmpty(channelName) || string.IsNullOrEmpty(accessToken)) {
                Console.WriteLine("Channel Name or Access Token is empty\r\nPress any key to exit");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Wait for beatoraja connect");
            beatorajaPipe.WaitForConnection();
            pipeWriter.AutoFlush = true;
            Console.WriteLine("beatoraja Client has been connected\r\nPress any key to exit");
            _ = new TwitchBot(channelName, accessToken);
        
            Console.ReadLine();
            pipeWriter.Dispose();
            beatorajaPipe.Dispose();
        }
    }
}
