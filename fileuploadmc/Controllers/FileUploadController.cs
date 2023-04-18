using fileuploadmc.DTO;
using fileuploadmc.Exceptions;
using fileuploadmc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace fileuploadmc.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private IFileHandler _fileHandler;
        private IExcelReader _excelReader;
        private IConfiguration _configuration;

        public FileUploadController(IFileHandler fileHandler,IExcelReader excelReader, IConfiguration configuration)
        {
            _fileHandler = fileHandler;
            _excelReader = excelReader;
            _configuration = configuration;
        }
        [HttpPost("uploadphoto")]
        public async Task<IActionResult> UploadPhotoAsync([FromForm] UploadPhotoRequest file)
        {
            try
            {
                var res = await _fileHandler.storeFile(file.File);
                foreach (QuestionDTO q in _excelReader.ReadExcelToQuestions(res))
                {
                    using (var httpClient = new HttpClient())
                    {
                        string requestStr = _configuration.GetValue<string>("QuizEngineMC") + "/question";
                        var content = JsonContent.Create(q);
                        var task = await httpClient.PostAsync(requestStr, content);
                        var str = await task.Content.ReadAsStringAsync();
                        Console.WriteLine(str);

                    }
                }
                
                return Ok(_excelReader.ReadExcelToQuestions(res));


            }
            catch (FileInvalidException ex)
            {
                return Unauthorized(ex.Message);

            }


        }
    }
}
