using YoutubeToMp3.WebServer;

namespace YoutubeToMp3;

class Program
{
    static async Task Main(string[] args)
    {
        WebServerController webServerController = new();

        webServerController.StartServer();
        
        await Task.Delay(Timeout.Infinite);
    }
}