using fileuploadmc.Exceptions;
using fileuploadmc.Helpers;
using nClam;
using System.Net;

namespace fileuploadmc.Services
{
    public class FileHandlerHelper : IFileHandler
    {
        private readonly long _fileSizeLimit;
        private readonly string _pathStore;
        private IConfiguration _configuration;
        private static IEnumerable<GenericFileType> Types { get; set; }

        public FileHandlerHelper(IConfiguration config)
        {
            _configuration = config;
            _pathStore = _configuration["PathFile"];
            _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");
            Types = new List<GenericFileType>
            {
                //new Jpeg(),
               // new Png(),
                new XLS(),
                new XLSX()
            }
             .OrderByDescending(x => x.SignatureLength)
             .ToList();
        }
        public async Task<string> storeFile(IFormFile Filef)
        {
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                        Filef.FileName);
            Console.Write(_configuration["PathFile"]);
            var res = _configuration.GetValue<string>("PathFile");
            string randomName = Path.GetRandomFileName().Replace('.', '_') + Path.GetExtension(Filef.FileName);

            var filePath = Path.Combine(_pathStore,
            randomName);

            if (Filef.Length > _fileSizeLimit)
            {
                throw new FileInvalidException("File is too Large", trustedFileNameForDisplay);
            }
            if (!ValidateType(Filef))
            {
                throw new FileInvalidException("File type invalid", trustedFileNameForDisplay);
            }
            var result = await ScanFileAsync(Filef);
            Console.Write(result); 
            using (var stream = File.Create(filePath))
            {
                await Filef.CopyToAsync(stream);
            }
            return filePath;
        }

        private bool ValidateType(IFormFile Filef)
        {
            FileTypeVerifyResult result = new FileTypeVerifyResult
            {
                Name = "Unknown",
                Description = "Unknown File Type",
                IsVerified = false
            };
            string type = Path.GetExtension(Filef.FileName).ToLower();
            type = type.Remove(0, 1);
            List<string> detectedExtensions = new List<string>();
            foreach (var fileType in Types)
            {
                result = fileType.Verify(Filef);
                if (result.IsVerified)
                {
                    detectedExtensions = fileType.Extensions;
                    break;
                }

            }
            if (result.IsVerified && detectedExtensions.Contains(type))
            {
                return true;
            }
            return false;

        }
        private async Task<string> ScanFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "File is empty";

            var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            byte[] fileBytes = ms.ToArray();
            string Result = string.Empty;
            try
            {
                var clam = new ClamClient(this._configuration["ClamAVServer:URL"],
                Convert.ToInt32(this._configuration["ClamAVServer:Port"]));
                var scanResult = await clam.SendAndScanFileAsync(fileBytes);

                // Switch Expression C# 8.0   
                Result = scanResult.Result switch
                {
                    ClamScanResults.Clean => "Clean",
                    ClamScanResults.VirusDetected => "Virus Detected",
                    ClamScanResults.Error => "Error in File",
                    ClamScanResults.Unknown => "Unknown File",
                    _ => "No case available"
                };
            }
            catch (Exception ex)
            {
                return "AntiVirus Not Connected";
            }

            return Result;
        }

    }
}
