namespace YoutubeToMp3;

class Program
{
    static async Task Main(string[] args)
    {
        _converter = new ConvertController();
        _converter.Convert(
            "/Users/cykim/Downloads/",
            // "https://www.youtube.com/watch?v=yT9V6toG5bk",
            "https://youtu.be/PhMEk6xbWyY?si=jPphcTj-FXZ_9DwO",
            "test"
            );
        
        await Task.Delay(Timeout.Infinite);
    }

    private static ConvertController? _converter;
}