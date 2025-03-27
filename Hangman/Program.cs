using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Player
{
    public string Name { get; private set; }
    public int Score { get; private set; }
    public int Lives { get; set; }

    public Player(string name, int lives)
    {
        Name = name;
        Score = 0;
        Lives = lives;
    }

    public void IncreaseScore()
    {
        Score += 10;
    }

    // ซื้อชีวิตเพิ่ม
    public void BuyExtraLife()
    {
        if (Score >= 50)
        {
            Lives++;
            Score -= 50;
            Console.WriteLine("❤️ Extra Life Purchased!");
        }
        else
        {
            Console.WriteLine("❌ Not enough points to buy an extra life.");
        }
    }
}

class WordBank
{
    private List<string> words = new List<string>();
    private List<string> hints = new List<string>();

    public WordBank()
    {
        LoadWordsFromFile("words.txt");
    }

    private void LoadWordsFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    words.Add(parts[0].Trim());
                    hints.Add(parts[1].Trim());
                }
            }
        }
        else
        {
            Console.WriteLine("⚠️ Word file not found! Using default words.");
            words = new List<string> { "hello", "world", "computer" };
            hints = new List<string> { "A greeting", "A place we live", "An electronic device" };
        }
    }

    public (string word, string hint) GetWord(int level)
    {
        if (level - 1 < words.Count)
            return (words[level - 1], hints[level - 1]);
        return ("", "");
    }

    public int GetTotalLevels() => words.Count;
}

class Scoreboard
{
    private List<Player> players;

    public Scoreboard()
    {
        players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void DisplayLeaderboard()
    {
        Console.WriteLine("\n🏆 Leaderboard 🏆");
        foreach (var player in players.OrderByDescending(p => p.Score))
        {
            Console.WriteLine($"{player.Name}: {player.Score} points");
        }
    }
}

class HangmanGame
{
    private WordBank wordBank;
    private string word;
    private string hint;
    private HashSet<char> guessedLetters;
    private int maxLives = 7;
    private int level = 1;
    private List<Player> players;
    private int currentPlayerIndex = 0;
    private Scoreboard scoreboard;

    public HangmanGame()
    {
        players = new List<Player>();
        scoreboard = new Scoreboard();

        Console.Write("Enter Player 1 name: ");
        players.Add(new Player(Console.ReadLine(), maxLives));
        Console.Write("Enter Player 2 name: ");
        players.Add(new Player(Console.ReadLine(), maxLives));

        scoreboard.AddPlayer(players[0]);
        scoreboard.AddPlayer(players[1]);

        wordBank = new WordBank();
        guessedLetters = new HashSet<char>();
        StartNewLevel();
    }

    private void StartNewLevel()
    {
        (word, hint) = wordBank.GetWord(level);
        foreach (var player in players)
        {
            player.Lives = maxLives - (level - 1);
        }
        guessedLetters.Clear();
    }

    public void Play()
    {
        while (level <= wordBank.GetTotalLevels())
        {
            while (players[currentPlayerIndex].Lives > 0)
            {
                DisplayGameStatus();

                if (IsWordComplete())
                {
                    Console.WriteLine($"\n🎉 {players[currentPlayerIndex].Name} completed level {level}! 🎉");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    level++;
                    if (level <= wordBank.GetTotalLevels()) StartNewLevel();
                    break;
                }

                char guess = GetPlayerGuess();
                ProcessGuess(guess);
            }

            if (players[currentPlayerIndex].Lives == 0)
            {
                Console.WriteLine($"\n💀 {players[currentPlayerIndex].Name} has lost all lives! Game Over! 💀");
                break;
            }
        }

        Console.WriteLine("\n🎊 Final Scores 🎊");
        foreach (var player in players)
        {
            Console.WriteLine($"{player.Name}: {player.Score} points");
        }

        scoreboard.DisplayLeaderboard();
        Console.WriteLine("\n🎉 Thanks for playing! 🎉");
    }

    private void DisplayGameStatus()
    {
        Console.Clear();
        Player currentPlayer = players[currentPlayerIndex];

        Console.WriteLine("========================================");
        Console.WriteLine("            HANGMAN GAME");
        Console.WriteLine("========================================\n");
        Console.WriteLine($"Hint: {hint}");
        Console.Write("Word: ");

        foreach (char c in word)
        {
            Console.Write(guessedLetters.Contains(c) ? $"{c} " : "_ ");
        }

        Console.WriteLine("\n----------------------------------------");
        Console.WriteLine($"Level: {level}");
        Console.WriteLine($"Lives: {currentPlayer.Lives}/{maxLives}");
        Console.WriteLine($"Player: {currentPlayer.Name} | Score: {currentPlayer.Score}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("(1) Guess Letter | (2) Buy Extra Life");
    }

    private bool IsWordComplete()
    {
        foreach (char c in word)
        {
            if (!guessedLetters.Contains(c))
                return false;
        }
        return true;
    }

    private char GetPlayerGuess()
    {
        Console.Write("\nPlease guess a letter: ");
        char guess;
        while (!char.TryParse(Console.ReadLine(), out guess))
        {
            Console.Write("Invalid input. Please enter a single letter: ");
        }
        return char.ToLower(guess);
    }

    private void ProcessGuess(char guess)
    {
        Player currentPlayer = players[currentPlayerIndex];

        if (word.Contains(guess) && !guessedLetters.Contains(guess))
        {
            Console.WriteLine("\n✅ Correct!");
            guessedLetters.Add(guess);
            currentPlayer.IncreaseScore();
        }
        else if (!guessedLetters.Contains(guess))
        {
            Console.WriteLine("\n❌ Incorrect!");
            currentPlayer.Lives--;
            guessedLetters.Add(guess);
            SwitchTurn();
        }
        else
        {
            Console.WriteLine("\n⚠️ You've already guessed that letter!");
        }

        // Option to buy extra life
        if (currentPlayer.Lives > 0)
        {
            Console.WriteLine("\nDo you want to buy an extra life? (Y/N)");
            string choice = Console.ReadLine();
            if (choice?.ToUpper() == "Y")
            {
                currentPlayer.BuyExtraLife();
            }
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void SwitchTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }
}

class Program
{
    static void Main()
    {
        HangmanGame game = new HangmanGame();
        game.Play();
    }
}
