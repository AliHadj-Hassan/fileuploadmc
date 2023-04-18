using fileuploadmc.Services;
using Grpc.Core;
using fileuploadmc.Helpers;
using fileuploadmc.GrpcServices;
using fileuploadmc.Exceptions;

namespace fileuploadmc.GrpcControllers
{
    public class GreeterService:Greeter.GreeterBase
    {
        private IFileHandler _fileHandler;

        public GreeterService(IFileHandler fileHandler) : base()
        {
            _fileHandler = fileHandler;

        }
   
        public override async Task<FileReply> SayHello(FileRequest request, ServerCallContext context)
        {
            Console.WriteLine("I am here");
            var stream = new MemoryStream(request.Data.ToByteArray());
            IFormFile file = new FormFile(stream, 0, request.Data.ToByteArray().Length, "name", request.Name);
            try
            {
                string res = await _fileHandler.storeFile(file);
                return await Task.FromResult(new FileReply { Message = res });
            }
            catch (FileInvalidException ex)
            {
                 return await Task.FromResult(new FileReply { Message = ex.Message });

            }
            
        }
    }
  

}
