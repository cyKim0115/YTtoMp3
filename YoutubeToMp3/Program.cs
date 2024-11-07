using YoutubeToMp3.WebServer;

namespace YoutubeToMp3;

class Program
{
    static async Task Main(string[] args)
    {
        var convert = new ConvertController();
        convert.Convert("/Users/cykim/Downloads/",
            "videoUrl",
            "youtube");

        WebServerController webServerController = new();

        webServerController.StartServer();

        await Task.Delay(Timeout.Infinite);
    }
}