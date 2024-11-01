using System.Net;

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
            Thread thread = new Thread(() => ProcessRequest(context));
            thread.IsBackground = true;
            thread.Start();
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        string? str = context.Request.RawUrl?.Split('/')[1];

        switch (str)
        {
            case "Convert":
                _route.PostConvert(context);
                break;
            case "List":
                break;
            default:
                Console.WriteLine($"없는 요청 {str}");
                break;
        }
    }
}