using System;


namespace HangmanEX
{
    class WordBank
    {
        private static readonly string[] words = { "cat", "dog", "horse", "eagle", "pig", "elephant" ,"lion","cheese","falafel","banana","apple","orange"};
        private static readonly Random r = new Random();

        public static string ReturnWord()
        {
            return words[r.Next(words.Length)];
        }
    }
}
