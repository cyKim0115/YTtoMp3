using System.Net;
using Newtonsoft.Json;
using YDLib;

#pragma warning disable CS8602
#pragma warning disable CS8600

namespace YoutubeToMp3.WebServer;

internal partial class WebServerRoute
{
    public async Task GetDownload(HttpListenerContext context)
    {
        var fileName = context.Request.RawUrl?.Split('/')[2];
        var fileFullPath = Path.Join(GlobalFunction.GetDownloadPath(), fileName);

        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        
        try
        {
            // 해당 파일이 없다면 리턴
            if (!File.Exists(fileFullPath))
            {
                response.OutputStream.Close();
                return;
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "audio/mpeg";
            response.ContentLength64 = new FileInfo(fileFullPath).Length;
            response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            await using (FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
            {
                await fs.CopyToAsync(response.OutputStream);
            }

            response.OutputStream.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.OutputStream.Close();
        }
    }
}