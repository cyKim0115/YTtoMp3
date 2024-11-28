using System.Net;
using System.Text;
using Newtonsoft.Json;
using YDLib;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public (HttpListenerResponse response, string data) GetCheck(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        string responseMessage = "Request completed successfully.";
        
        (HttpListenerResponse response, string data) result = (response, responseMessage);
        return result;
    }
}