using VideoLibrary;

namespace YoutubeToMp3;

public class ConvertController
{
    // 정상 작동 되는 FFmpeg의 aac to mp3 커맨드
    // ffmpeg -i inputfile.m4a -c:a libmp3lame -ac 2 -q:a 2 outputfile.mp3

    private readonly AacToMp3Converter _mp3Converter = new AacToMp3Converter();

    public async void Convert(string savePath, string videoUrl, string mp3Name)
    {
        Console.WriteLine("Convert: Start Process");

        var result = await GetVideo(videoUrl, savePath);

        _mp3Converter.ConvertWithFFmpeg(result, savePath);

        Console.WriteLine("Convert: End Process");
    }

    private Task<VideoInfo> GetVideo(string videoUrl, string savePath)
    {
        CustomYouTube youtube = new CustomYouTube();
        IEnumerable<YouTubeVideo>? videos = youtube.GetAllVideosAsync(videoUrl).GetAwaiter().GetResult();
        List<YouTubeVideo> listVideo = videos.Where(i => i.AdaptiveKind == AdaptiveKind.Audio).ToList();

        if (listVideo.Count == 0)
        {
            Console.WriteLine("오디오 타입의 비디오가 없습니다.");
            
            return Task.FromResult<VideoInfo>(null);
        }
        
        int maxBitrate = listVideo.Max(x => x.AudioBitrate);
        YouTubeVideo targetVideo = listVideo.First(x => x.AudioBitrate == maxBitrate);

        string fileName = $"{targetVideo.FullName}";
        youtube.CreateDownloadAsync(
                new Uri(targetVideo.Uri),
                Path.Combine(savePath, fileName),
                new Progress<Tuple<long, long>>(v =>
                {
                    var percent = (int)((v.Item1 * 100) / v.Item2);
                    Console.Write("Downloading.. ( % {0} ) {1} / {2} MB\r", percent, (v.Item1 / (double)(1024 * 1024)).ToString("N"), (v.Item2 / (double)(1024 * 1024)).ToString("N"));
                }))
            .GetAwaiter().GetResult();

        Console.WriteLine($"Download Complete {fileName}");

        return Task.FromResult(new VideoInfo(targetVideo));
    }
}