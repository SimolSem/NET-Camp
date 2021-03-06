using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_06_2
{
    public class StringReaderCamp
    {
        #region properties
        public string FileName { get; set; } = "";
        #endregion

        #region delegates
        public Action<string>? ExtAction;
        #endregion

        #region constructors
        public StringReaderCamp() { }
        public StringReaderCamp(string fileName) : this()
        {
            FileName = fileName;
        }
        #endregion

        #region methods
        public void ShowContent()
        {
            try
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string? strTmp = sr.ReadLine();
                        if (strTmp != null)
                        {
                            ExtAction?.Invoke(strTmp);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ExtAction?.Invoke($"File '{FileName}' not found");
            }
            catch (Exception e)
            {
                ExtAction?.Invoke(e.Message);
            }
        }

        public List<string> SplitеTextIntoSentences()
        {
            List<string> result = new();

            try
            {
                using (StreamReader sr = new StreamReader(FileName))
                { 
                    string currentSentence = "";
                    while (!sr.EndOfStream)
                    {
                        string? lastLineRead = sr.ReadLine();
                        if (!string.IsNullOrEmpty(lastLineRead))
                        {
                            string[] subLine = lastLineRead.Split(".", StringSplitOptions.TrimEntries);

                            //У currentLine в минулої ітерації циклу прийходить початок незавершенного речення.
                            //до нього додаємо  subLine[0] як його продовження
                            currentSentence = string.IsNullOrEmpty(currentSentence) ? subLine[0] : $"{currentSentence} {subLine[0]}";

                            //Якшо subLine має больше 1 єлементу, то в прочинаному lastLineRead є кінець речення и можно його записати у result 
                            if (subLine.Length > 1)
                            {
                                //Нульовий у currentLine, а останній обходимо, а ле у файл не пишемо, а запам'ятовуемо у currentLine
                                for (int i = 1; i < subLine.Length; i++)
                                {
                                    result.Add(currentSentence);
                                    //У currentLine кладемо початок наступного речення
                                    currentSentence = subLine[i];
                                }
                            }
                        }
                    }
                    //Якшщо у currentLine шось залишилось, то вважаемо це окремим реченням яке було без крапки и записуємо його
                    if (!string.IsNullOrEmpty(currentSentence)) 
                    {
                        result.Add(currentSentence);
                    }

                }
            }
            catch (FileNotFoundException)
            {
                ExtAction?.Invoke($"File '{FileName}' not found");
            }
            catch (Exception e)
            {
                ExtAction?.Invoke(e.Message);
            }

            return result;

        }

        public void SaveToFile(string nameOutputFile)
        {
            SaveToFile(nameOutputFile, SplitеTextIntoSentences());
        }
        public void SaveToFile(string nameOutputFile, List<string> sentences)
        {

            try
            {
                using (StreamWriter sw = new StreamWriter(nameOutputFile))
                {
                    foreach(string sentence in sentences)
                    {
                        sw.WriteLine($"{sentence}."); 
                    }
                }
            }
            catch (Exception e)
            {
                ExtAction?.Invoke(e.Message);
            }
        }
                            
        public static (string, string) ShortLongWords(string str)
            {
                string[] tmpStr = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (tmpStr.Length == 0)
                {
                    return ("", "");
                }

                string shortStr = tmpStr[0];
                string longStr = tmpStr[0];

                for(int i = 1; i < tmpStr.Length; i++)
                {
                    if (tmpStr[i].Length > longStr.Length)
                    {
                        longStr = tmpStr[i];
                    }
                    if (tmpStr[i].Length < shortStr.Length)
                    {
                        shortStr = tmpStr[i];
                    }
                }
                return (shortStr, longStr);

            }
        
        public void ShowShortLongWords()
        {
            ShowShortLongWords(SplitеTextIntoSentences());
        }
        public void ShowShortLongWords(List<string> sentences)
        {
            foreach (string sentence in sentences)
            {
                ExtAction?.Invoke($"{sentence}.{StringReaderCamp.ShortLongWords(sentence).ToString()}");
            }
        }



        #endregion

    }
}
