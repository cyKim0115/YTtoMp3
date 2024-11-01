using FFMpegCore;
using FFMpegCore.Enums;
using VideoLibrary;

namespace YoutubeToMp3;

public class VideoInfo
{
    public string name;
    
    public string format;
    
    public int bitrate;

    public VideoInfo(YouTubeVideo video)
    {
        name = video.FullName;
        format = video.AudioFormat.ToString().ToLower();
        bitrate = video.AudioBitrate;
    }

    public Codec GetCodec()
    {
        Console.WriteLine($"code is {format}");
        
        return FFMpeg.GetCodec(format);
    }
}