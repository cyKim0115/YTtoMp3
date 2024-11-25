using YoutubeToMp3.WebServer;
using static YoutubeToMp3.GlobalFunction;

namespace YoutubeToMp3;

#pragma warning disable CS8602
class Program
{
    static async Task Main(string[] args)
    {
        WebServerController webServerController = new();

        webServerController.StartServer();

        await Task.Delay(Timeout.Infinite);
    }
}