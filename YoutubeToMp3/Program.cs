namespace YoutubeToMp3;

class Program
{
    static void Main(string[] args)
    {
        converter = new YoutubeConverter();
        converter.Convert(
            "/Users/cyKim/Downloads/",
            // "https://www.youtube.com/watch?v=yT9V6toG5bk",
            "https://youtu.be/sfFjzKxJmUA",
            "test"
            );
        
        Console.ReadLine();
    }

    private static YoutubeConverter converter;
}