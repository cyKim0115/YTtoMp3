using System.Net;
using System.Text;

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public async Task<(HttpListenerResponse response, string data)> GetDownload(HttpListenerContext context)
    {
        Stream body = context.Request.InputStream;
        Encoding encoding = context.Request.ContentEncoding;
        StreamReader reader = new(body, encoding);
        string strReceive = reader.ReadToEnd();

        var fileName = "어항[을 깨다:부시다] - 뢴트게늄.mp3";
        var fileFullPath = Path.Join(GlobalFunction.GetDownloadPath(), fileName);

        
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        
        Console.WriteLine($"Listen {strReceive}");

        if (!File.Exists(fileFullPath))
        {
            return (response, "Failed");
        }

        (HttpListenerResponse response, string data) result;
        try
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "audio/mpeg";
            response.ContentLength64 = new System.IO.FileInfo(fileFullPath).Length;
            response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            await using (FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
            {
                await fs.CopyToAsync(response.OutputStream);
            }
            
            response.OutputStream.Close();
        
            result = (response, "Success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.OutputStream.Close();
            
            result = (response, "Success");
        }
        
        return result;
    }
}