using System.Net;
using Newtonsoft.Json;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public (HttpListenerResponse response, string data) GetList(HttpListenerContext context)
    {
        var files = Directory.GetFiles(GlobalFunction.GetDownloadPath());

        List<FileInfo> listFileInfo = new();
        foreach (string fileUri in files)
        {
            listFileInfo.Add(new FileInfo(fileUri));
        }

        string logResult = JsonConvert.SerializeObject(listFileInfo);
        
        Console.WriteLine(logResult);
        
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        
        (HttpListenerResponse response, string data) result = (response, logResult);
        return result;
    }
}