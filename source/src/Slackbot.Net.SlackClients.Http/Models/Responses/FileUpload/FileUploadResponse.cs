namespace Slackbot.Net.SlackClients.Http.Models.Responses.FileUpload;

public class FileUploadResponse : Response
{
    public FileUploadFile File { get; set; }
}

public class FileUploadFile
{
    public string Id { get; set; }
    public int Created { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string FileType { get; set; }
    public string Pretty_Type { get; set; }
    public string User { get; set; }
    public bool Is_Public { get; set; }
}