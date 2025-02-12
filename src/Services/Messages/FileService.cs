namespace BlazorSecretManager.Services.Messages;

public interface IFileService
{
    Task<bool> UploadFile(byte[] fileContent, string fileName, string contentType);
}

public class FileService : IFileService
{
    public FileService()
    {
        
    }

    public async Task<bool> UploadFile(byte[] fileContent, string fileName, string contentType)
    {
        var filePath = Path.Combine("./wwwroot/images", fileName);
        await File.WriteAllBytesAsync(filePath, fileContent);
        return true;
    }
}