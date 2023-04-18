
using fileuploadmc.DTO.Enums;

namespace fileuploadmc.DTO
{
    public class QuestionDTO
    {


        public string Name { get; set; }
        public string Tag { get; set; }
        public int Difficulty { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public List<string> Answers { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> WrongAnswers { get; set; }
        public QuestionDTO(string name, string tag, int difficulty, string language, string type, List<string> answers, List<string> correctAnswers)
        {
            Name = name;
            Tag = tag;
            Difficulty = difficulty;
            Language = language;
            Type = type;
            Answers = answers;
            CorrectAnswers = correctAnswers;
            WrongAnswers = answers.Except(correctAnswers).ToList();
        }

    }
}
