namespace fileuploadmc.Helpers
{
    public abstract class GenericFileType
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public List<string> Extensions { get;  }
            = new List<string>();

        private List<byte[]> Signatures { get; }
            = new List<byte[]>();

        public int SignatureLength => Signatures.Max(m => m.Length);

        protected GenericFileType AddSignatures(params byte[][] bytes)
        {
            Signatures.AddRange(bytes);
            return this;
        }

        protected GenericFileType AddExtensions(params string[] extensions)
        {
            Extensions.AddRange(extensions);
            return this;
        }

        public FileTypeVerifyResult Verify(IFormFile file)
        {
            Stream stream=file.OpenReadStream();
            var reader = new BinaryReader(stream);
            var headerBytes = reader.ReadBytes(SignatureLength);

            return new FileTypeVerifyResult
            {
                Name = Name,
                Description = Description,
                IsVerified = Signatures.Any(signature =>
                    headerBytes.Take(signature.Length)
                        .SequenceEqual(signature)
                )
            };
        }
    }

    public class FileTypeVerifyResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; }
    }
}
