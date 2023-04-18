namespace fileuploadmc.Services
{
    public interface IFileHandler
    {
        Task<string> storeFile(IFormFile Filef);
        //bool ValidateType(IFormFile Filef);
    }
}
