using System.Net;
using System.Text;
using Newtonsoft.Json;
using YDLib;
#pragma warning disable CS8602
#pragma warning disable CS8600

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    private readonly ConvertController _convertController = new();
    
    public (HttpListenerResponse response, string data) PostConvert(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        string responseMessage = "Request completed successfully.";
        
        try
        {
            Stream body = context.Request.InputStream;
            Encoding encoding = context.Request.ContentEncoding;
            StreamReader reader = new(body, encoding);
            string strReceive = reader.ReadToEnd();

            ConvertRequestInfo requestInfo = JsonConvert.DeserializeObject<ConvertRequestInfo>(strReceive);
            Thread newThread = new(Task.Run(() => { _convertController.Convert(requestInfo.url, requestInfo.name); }).GetAwaiter().GetResult)
                {
                    IsBackground = true
                };
            newThread.Start();
            
            Console.WriteLine($"Listen {strReceive}");
        }
        catch (Exception e)
        {
            response.StatusCode = 9000;
            responseMessage = $"Request could not be processed: {e.Message}";
        }

        Console.WriteLine($"{response.StatusCode}\n{DateTime.Now}\n{responseMessage}");
        (HttpListenerResponse response, string data) result = (response, responseMessage);
        return result;
    }
}