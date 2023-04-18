using ExcelDataReader;
using fileuploadmc.DTO;

namespace fileuploadmc.Services
{
    public class ExcelReader : IExcelReader
    {
        public IEnumerable<QuestionDTO> ReadExcelToQuestions(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        reader.Read();
                        while (reader.Read()) //Each ROW
                        {
                            string name = reader.GetString(0);
                            string tag = reader.GetString(1);
                            int difficulty = 0;
                            if (reader.GetValue(2) != null)
                            {
                                 difficulty = int.Parse(reader.GetValue(2).ToString());

                            }

                            string lang = reader.GetString(3);
                            string type = reader.GetString(4);
                            List<string> answers = new List<string>();
                            List<string> correctAnswers = new List<string>();

                            for (int column = 5; column < reader.FieldCount; column++)
                            {
                                string answer = reader.GetValue(column).ToString();
                                answers.Add(answer);
                                if (answer.Last() == '*')
                                {
                                    correctAnswers.Add(answer);

                                }
                                //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                Console.WriteLine(reader.GetValue(column));//Get Value returns object
                            }
                            yield return new QuestionDTO(name, tag, difficulty, lang, type, answers, correctAnswers);

                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET
                    yield break;

                }

            }
            
        }
    }
}
