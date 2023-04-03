namespace Slackbot.Net.SlackClients.Http.Models.Requests.FileUpload;

public class FileUploadRequest
{
    public string Channels { get; set; }
    public string Content { get; set; }
    public string Filename { get; set; }
    public string Filetype { get; set; }
    public string Initial_Comment { get; set; }
    public string Thread_Ts { get; set; }
    public string Title { get; set; }
}

public class FileUploadMultiPartRequest
{
    public string Channels { get; set; }
    public Byte[] File { get; set; }
    public string Filename { get; set; }
    public string Filetype { get; set; }
    public string Initial_Comment { get; set; }
    public string Thread_Ts { get; set; }
    public string Title { get; set; }
}