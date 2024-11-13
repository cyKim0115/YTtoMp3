using FFMpegCore;
using FFMpegCore.Enums;
#pragma warning disable CS8602

namespace YoutubeToMp3;

public class AacToMp3Converter
{
    public void ConvertWithFFmpeg(VideoInfo? info, string savePath, string fileName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = $"{info.name}";

            string inputFilePath = Path.Combine(savePath, fileName);
            string outputFilePath = Path.Combine(savePath, $"{fileName}.mp3");

            // 아웃 풋이 이미 있다면 지우고 다시 변환할거임
            if(File.Exists(outputFilePath))
                File.Delete(outputFilePath);
            
            FFMpegArguments
                .FromFileInput(inputFilePath)
                .OutputToFile(outputFilePath, false, options => options
                    .WithAudioCodec(info.GetCodec())
                    .WithAudioCodec(AudioCodec.LibMp3Lame)
                    .WithAudioBitrate((AudioQuality)info.bitrate)
                    .WithAudioSamplingRate(44100))
                .WithLogLevel(FFMpegLogLevel.Info)
                .ProcessSynchronously();
            
            // 사용하고난 인풋은 제거
            if (File.Exists(inputFilePath))
                File.Delete(inputFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}