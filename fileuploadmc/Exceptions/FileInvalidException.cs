namespace fileuploadmc.Exceptions
{
    public class FileInvalidException : Exception
    {
        private string fileName { get; set; }
        
        public FileInvalidException(string fileName):base()
        {
           this.fileName = fileName;
        }

        public FileInvalidException(string message,string fileName)
            : base(message)
        {
            this.fileName = fileName;
        }

        public FileInvalidException(string message, Exception inner,string fileName)
            : base(message, inner)
        {
            this.fileName = fileName;
        }
    }
}
