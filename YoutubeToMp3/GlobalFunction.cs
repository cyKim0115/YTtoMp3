namespace YoutubeToMp3;

public class GlobalFunction
{
    public static string GetDownloadPath()
    {
        string result = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName,
            "Mp3Downloads");

        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }

        return result;
    }
}