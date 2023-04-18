namespace fileuploadmc.Helpers
{
    public sealed class Jpeg : GenericFileType
    {
        public Jpeg()
        {
            Name = "JPEG";
            Description = "JPEG IMAGE";
            AddExtensions("jpeg", "jpg");
            AddSignatures(
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
            );
        }
    }

    public sealed class XLS : GenericFileType
    {
        public XLS()
        {
            Name = "XLS";
            Description = "Excel File";
            AddExtensions("xls");
            AddSignatures(
                new byte[] { 0xD0, 0xCF, 0x11 ,0xE0 ,0xA1 ,0xB1, 0x1A, 0xE1 }
            );
        }
    }
    public sealed class XLSX : GenericFileType
    {
        public XLSX()
        {
            Name = "XLSX";
            Description = "Excel File X";
            AddExtensions("xlsx");
            AddSignatures(
                new byte[] { 0x50, 0x4B, 0x03, 0x04 }
            );
        }
    }

    public sealed class Png : GenericFileType
    {
        public Png()
        {
            Name = "PNG";
            Description = "PNG Image";
            AddExtensions("png");
            AddSignatures(
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
            );
        }
    }
}
