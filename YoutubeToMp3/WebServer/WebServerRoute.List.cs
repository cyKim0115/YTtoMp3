using System.Net;
using Newtonsoft.Json;
using YDLib;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public (HttpListenerResponse response, string data) GetList(HttpListenerContext context)
    {
        var files = Directory.GetFiles(GlobalFunction.GetDownloadPath());

        List<FileItem> listFileInfo = new();
        foreach (string fileUri in files)
        {
            var fileInfo = new FileItem(fileUri);
            if(fileInfo.extension != ".mp3")
                continue;
                
            listFileInfo.Add(fileInfo);
        }

        string logResult = JsonConvert.SerializeObject(listFileInfo);
        
        Console.WriteLine(logResult);
        
        HttpListenerResponse response = context.Response;
        
        (HttpListenerResponse response, string data) result = (response, logResult);
        return result;
    }
}