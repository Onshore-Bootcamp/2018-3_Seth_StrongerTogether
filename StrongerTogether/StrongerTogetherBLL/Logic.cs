using StrongerTogetherBLL.Model;
using System.Collections.Generic;


namespace StrongerTogetherBLL
{
    public class Logic
    {
        // adding the words to an array by using spaces to seperate the words and counting them
        public int CountWords(string content)
        {
            string[] words = content.Split(' ');
            int count = words.Length;
            return count;
        }

        public float AverageWordCount(List<WrittingsBO> allWrittings)
        {
            //Create total amount of words for all writtings.
            float sum = 0;
            float Average = 0;

            if (allWrittings.Count > 0)
            {
                foreach (WrittingsBO writtings in allWrittings)
                {
                    sum += CountWords(writtings.Content);
                }

                //Take total number of words and divide by number of writtings.
                Average = sum / allWrittings.Count; 
            }

            return Average;
        }

        
    }
}
