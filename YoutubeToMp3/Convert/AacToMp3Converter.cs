using FFMpegCore;
using FFMpegCore.Enums;

namespace YoutubeToMp3;

public class AacToMp3Converter
{
    public void ConvertWithFFmpeg(VideoInfo? info, string savePath)
    {
        try
        {
            if(File.Exists(Path.Combine(savePath, $"{info.name}.mp3")))
                File.Delete(Path.Combine(savePath, $"{info.name}.mp3"));
            
            FFMpegArguments
                .FromFileInput(Path.Combine(savePath, info.name))
                .OutputToFile(Path.Combine(savePath, $"{info.name}.mp3"), false, options => options
                    .WithAudioCodec(info.GetCodec())
                    .WithAudioCodec(AudioCodec.LibMp3Lame)
                    .WithAudioBitrate((AudioQuality)info.bitrate)
                    .WithAudioSamplingRate(44100))
                .WithLogLevel(FFMpegLogLevel.Info)
                .ProcessSynchronously();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}