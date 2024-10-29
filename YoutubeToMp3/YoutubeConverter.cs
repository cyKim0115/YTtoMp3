using System.Runtime.CompilerServices;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace YoutubeToMp3;

public class YoutubeConverter
{   
    public void Convert(string savePath, string videoUrl, string mp3Name)
    {
        Console.WriteLine("1");
        var source = savePath;
        var youtube = YouTube.Default;
        var vid = youtube.GetVideo(videoUrl);
        Console.WriteLine("1_1");
        // var bytes = vid.GetBytes();
        Console.WriteLine("1_2");
        // File.WriteAllBytes(source + vid.FullName,bytes);

        Console.WriteLine("2");
        var inputFile = new MediaFile { Filename = source + vid.FullName };
        var outputFile = new MediaFile { Filename = $"{mp3Name}.mp3" };

        Console.WriteLine("3");
        using (var engine = new Engine())
        {
            try
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                ((IDisposable)engine).Dispose();
            }

            Console.WriteLine("4");
        }
    }
}