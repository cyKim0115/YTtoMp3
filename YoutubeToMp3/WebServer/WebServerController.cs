using System.Net;
using System.Text;
#pragma warning disable CS8604

namespace YoutubeToMp3.WebServer;

public class WebServerController
{
    private HttpListener _listener;
    private WebServerRoute _route = new WebServerRoute();

    public void StartServer()
    {
        if (_listener == null)
        {
            _listener = new HttpListener();
        }

        _listener = new HttpListener();
        _listener.Prefixes.Add("http://*:9227/");
        _listener.Start();

        while (_listener.IsListening)
        {
            var context = _listener.GetContext();
            var thread = new Thread(() => ProcessRequest(context))
            {
                IsBackground = true
            };
            thread.Start();
        }
    }

    private void SendResponse(HttpListenerResponse response, string data)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(data);
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        response.Close();
    }
    
    private void ProcessRequest(HttpListenerContext context)
    {
        string? str = context.Request.RawUrl?.Split('/')[1];
        (HttpListenerResponse response, string data) responseData = (null, null);

        // favicon 무시
        if (!string.IsNullOrEmpty(str) && str.Contains("favicon"))
            return;
        
        switch (str)
        {
            case "Convert":
                responseData = _route.PostConvert(context);
                SendResponse(responseData.response,responseData.data);
                break;
            case "List":
                responseData = _route.GetList(context);
                SendResponse(responseData.response,responseData.data);
                break;
            case "Download":
                responseData = _route.GetDownload(context).GetAwaiter().GetResult();
                break;
            default:
                Console.WriteLine($"없는 요청 {str}");
                SendResponse(responseData.response,responseData.data);
                break;
        }
    }
}