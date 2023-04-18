using fileuploadmc.DTO;

namespace fileuploadmc.Services
{
    public interface IExcelReader
    {
        IEnumerable<QuestionDTO> ReadExcelToQuestions(string path);
    }
}
