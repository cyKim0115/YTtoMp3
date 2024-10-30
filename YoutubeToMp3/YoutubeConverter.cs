using System.Runtime.CompilerServices;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using VideoLibrary;

namespace YoutubeToMp3;

public class YoutubeConverter
{
    public async void Convert(string savePath, string videoUrl, string mp3Name, bool useCustomClient = true)
    {
        Console.WriteLine("Convert: Start Process");
        string source = savePath;
        List<string> downloadFileNames = new();

        if (!useCustomClient)
        {
            YouTubeVideo? vid = await GetVideo(videoUrl);

            Console.WriteLine($"Convert: Get Bytes ({vid.ContentLength}bytes)");
            var bytes = await vid.GetBytesAsync();

            Console.WriteLine("Convert: Write mp4 File");
            await File.WriteAllBytesAsync(source + vid.FullName, bytes);

            downloadFileNames.Add(vid.FullName);
        }
        else
        {
            // downloadFileName = await GetVideoWithCustomClient(videoUrl, savePath);
            var result = await GetVideoWithCustomClientTest(videoUrl,savePath);
            downloadFileNames.AddRange(result);
        }

        foreach (var name in downloadFileNames)
        {
            var inputFile = new MediaFile { Filename = Path.Combine(savePath, $"{name}") };
            var outputFile = new MediaFile { Filename = Path.Combine("/Users/cykim/Downloads", $"{name}.mp3") };

            using (var engine = new Engine("/opt/homebrew/bin/ffmpeg"))
            {
                try
                {
                    engine.ConvertProgressEvent += (sender, args) => { Console.WriteLine($"{sender} {args}"); };

                    Console.WriteLine("Convert: Get Metadata");
                    engine.GetMetadata(inputFile);

                    Console.WriteLine("Convert: Start Convert");
                    ConversionOptions options = new ConversionOptions()
                    {
                        AudioSampleRate = AudioSampleRate.Hz44100,
                    
                    };
                    engine.Convert(inputFile, outputFile, options);
                    engine.GetMetadata(outputFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Convert: Error {e}");
                    throw;
                }
                finally
                {
                    Console.WriteLine($"Convert: Convert Complete {name}");
                    ((IDisposable)engine).Dispose();
                }
            }   
        }
        
        Console.WriteLine("Convert: End Process");
    }

    private async Task<YouTubeVideo> GetVideo(string videoUrl)
    {
        var youtube = YouTube.Default;
        Console.WriteLine("Convert: Get Video from Youtube");
        Task<YouTubeVideo>? videoAsync = youtube.GetVideoAsync(videoUrl);
        TaskAwaiter<YouTubeVideo> awaiter = Task.Run(() => videoAsync).GetAwaiter();
        while (!awaiter.IsCompleted)
        {
            Console.WriteLine($"Wait...");
            await Task.Delay(1000);
        }

        YouTubeVideo result = awaiter.GetResult();

        return result;
    }

    private async Task<string> GetVideoWithCustomClient(string videoUrl, string savePath)
    {
        CustomYouTube youtube = new CustomYouTube();
        IEnumerable<YouTubeVideo>? videos = youtube.GetAllVideosAsync(videoUrl).GetAwaiter().GetResult();
        YouTubeVideo minResolution = videos.First(i => i.Resolution == videos.Min(j => j.Resolution));
        youtube.CreateDownloadAsync(
                new Uri(minResolution.Uri),
                Path.Combine(savePath,
                    minResolution.FullName),
                new Progress<Tuple<long, long>>((Tuple<long, long> v) =>
                {
                    var percent = (int)((v.Item1 * 100) / v.Item2);
                    Console.Write(string.Format("Downloading.. ( % {0} ) {1} / {2} MB\r", percent,
                        (v.Item1 / (double)(1024 * 1024)).ToString("N"),
                        (v.Item2 / (double)(1024 * 1024)).ToString("N")));
                }))
            .GetAwaiter().GetResult();

        return minResolution.FullName;
    }
    
    private async Task<List<string>> GetVideoWithCustomClientTest(string videoUrl, string savePath)
    {
        List<string> returnResult = new();
        CustomYouTube youtube = new CustomYouTube();
        IEnumerable<YouTubeVideo>? videos = youtube.GetAllVideosAsync(videoUrl).GetAwaiter().GetResult();
        var enumVideo = videos.Where(i => i.AdaptiveKind == AdaptiveKind.Audio);
        foreach (var video in enumVideo)
        {
            string fileNaem = $"{video.FullName}_{video.AudioFormat.ToString()}_{video.AudioBitrate}.{video.AudioFormat.ToString().ToLower()}";
            returnResult.Add(fileNaem);
            youtube.CreateDownloadAsync(
                    new Uri(video.Uri),
                    Path.Combine(savePath,fileNaem),
                    new Progress<Tuple<long, long>>((Tuple<long, long> v) =>
                    {
                        var percent = (int)((v.Item1 * 100) / v.Item2);
                        Console.Write(string.Format("Downloading.. ( % {0} ) {1} / {2} MB\r", percent,
                            (v.Item1 / (double)(1024 * 1024)).ToString("N"),
                            (v.Item2 / (double)(1024 * 1024)).ToString("N")));
                    }))
                .GetAwaiter().GetResult();
        }

        return returnResult;
    }
}