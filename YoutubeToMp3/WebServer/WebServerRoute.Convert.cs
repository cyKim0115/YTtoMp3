

using System.Net;
using System.Text;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public void PostConvert(HttpListenerContext context)
    {
        var response = context.Response;
        response.StatusCode = 200;
        
        Stream body = context.Request.InputStream;
        var encoding = context.Request.ContentEncoding;
        var reader = new StreamReader(body, encoding);
        string strReceive = reader.ReadToEnd();
        
        var buffer = Encoding.UTF8.GetBytes("ListenListen");
        response.ContentLength64 = buffer.Length;
        var output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        response.Close();
        
        
        Console.WriteLine($"ListenListen {strReceive}");
    }
}