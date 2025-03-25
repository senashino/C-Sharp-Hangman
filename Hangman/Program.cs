using System;
using System.Collections.Generic;

class Hangman
{
    static void Main()
    {
        List<string> words = new List<string> { "HELLO", "WORLD", "HANGMAN" , "PROGRAMING" }; // คำสำหรับแต่ละด่าน
        int maxLives = 7;

        foreach (string word in words)
        {
            int currentLives = maxLives;
            bool win = false;
            List<char> guessedLetters = new List<char>();

            while (currentLives > 0 && !win)
            {
                Console.WriteLine("\nEnter A-Z only to guess the word , Select one English letter at a time.");

                foreach (char c in word)
                {
                    if (guessedLetters.Contains(c))
                        Console.Write(c);
                    else
                        Console.Write("_");
                }

                Console.WriteLine("\nPlease guess a letter!");
                Console.WriteLine(currentLives + "/" + maxLives + " lives remaining.");

                char? guess = null;
                while (guess == null)
                {
                    string? input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        guess = Convert.ToChar(input);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please guess a letter!");
                    }
                }

                if (word.Contains(guess.Value) && !guessedLetters.Contains(guess.Value))
                    Console.WriteLine("Correct!");
                else
                {
                    Console.WriteLine("Incorrect!");
                    currentLives--;
                }
                guessedLetters.Add(guess.Value);

                bool wordComplete = true;

                foreach (char c in word)
                    if (!guessedLetters.Contains(c))
                        wordComplete = false;

                win = wordComplete;
            }

            if (win)
                Console.WriteLine("Congratulations, you passed this level!");
            else
            {
                Console.WriteLine("You lose...");
                break; // จบเกมถ้าผู้เล่นแพ้
            }
        }

        if (words.Last() == "hangman") // ตรวจสอบว่าผู้เล่นผ่านด่านสุดท้ายหรือไม่
            Console.WriteLine("Congratulations, you win the entire game!");
    }
}