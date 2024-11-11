

using System.Net;
using System.Text;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public (HttpListenerResponse response, string data) PostConvert(HttpListenerContext context)
    {
        Stream body = context.Request.InputStream;
        Encoding encoding = context.Request.ContentEncoding;
        StreamReader reader = new StreamReader(body, encoding);
        string strReceive = reader.ReadToEnd();
        
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        
        Console.WriteLine($"Listen {strReceive}");

        (HttpListenerResponse response, string data) result = (response, "Success");
        return result;
    }
}