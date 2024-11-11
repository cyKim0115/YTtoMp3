// ReSharper disable NotAccessedField.Global
// ReSharper disable InconsistentNaming
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace YoutubeToMp3.WebServer;

public class FileInfo
{
    private readonly float oneMega = 1048576;
    
    private string _fullPath;
    public string fileName;
    public string size;
    public string extension;

    public FileInfo(string fileURI)
    {
        System.IO.FileInfo fileInfo = new(fileURI);
        
        _fullPath = fileInfo.FullName;
        fileName = fileInfo.Name;
        size = (fileInfo.Length / oneMega).ToString("0.00")+ "MB";
        extension = fileInfo.Extension;
    }
}