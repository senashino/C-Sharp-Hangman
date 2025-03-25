using System;
using System.Collections.Generic;

class Hangman
{
    static void Main()
    {
        List<string> words = new List<string> { "hello", "world", "computer", "programming", "developer" };
        List<string> hints = new List<string> { "A common greeting", "The planet we live on", "An electronic device", "The act of writing code", "A person who writes software" };
        
        int maxLives = 7;
        int level = 1;
        bool win = false;

        while (level <= words.Count)
        {
            string word = words[level - 1];
            string hint = hints[level - 1];
            int currentLives = maxLives - (level - 1);
            List<char> guessedLetters = new List<char>();
            HashSet<char> hintLetters = new HashSet<char>();
            Random rand = new Random();

            while (hintLetters.Count < word.Length / 3)
            {
                hintLetters.Add(word[rand.Next(word.Length)]);
            }
            guessedLetters.AddRange(hintLetters);

            while (currentLives > 0)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("            HANGMAN GAME");
                Console.WriteLine("========================================\n");
                Console.WriteLine("Hint: " + hint);
                Console.Write("Word: ");
                bool wordComplete = true;

                foreach (char c in word)
                {
                    if (guessedLetters.Contains(c))
                        Console.Write(c + " ");
                    else
                    {
                        Console.Write("_ ");
                        wordComplete = false;
                    }
                }

                Console.WriteLine("\n----------------------------------------");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"Lives: {currentLives}/{maxLives}");
                Console.WriteLine("----------------------------------------");

                if (wordComplete)
                {
                    Console.WriteLine($"\n🎉 Congratulations! You completed level {level}! 🎉");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    level++;
                    break;
                }

                Console.Write("\nPlease guess a letter: ");
                char guess;
                while (!char.TryParse(Console.ReadLine(), out guess))
                {
                    Console.Write("Invalid input. Please enter a single letter: ");
                }
                guess = char.ToLower(guess);

                if (word.Contains(guess) && !guessedLetters.Contains(guess))
                {
                    Console.WriteLine("\n✅ Correct!");
                    guessedLetters.Add(guess);
                }
                else if (!guessedLetters.Contains(guess))
                {
                    Console.WriteLine("\n❌ Incorrect!");
                    currentLives--;
                    guessedLetters.Add(guess);
                }
                else
                {
                    Console.WriteLine("\n⚠️ You've already guessed that letter!");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            if (currentLives == 0)
            {
                Console.WriteLine("\n💀 You lose this level... Game Over! 💀");
                break;
            }
        }

        if (level > words.Count)
        {
            Console.WriteLine("\n🎊 Congratulations, you have completed all levels! 🎊");
        }
    }
}
