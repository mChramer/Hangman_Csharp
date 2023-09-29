using System.Net;
using System.Security.Principal;

class Program
{
    private static void Main(string[] args)
    {
        do
        {
            var words = CreateWordsList();
            var word = PickRandomWord(words);
            var usedLetters = string.Empty;
            var mask = InitMask(word.Length);
            var attempts = 6;
            
            RunGame(mask, usedLetters, attempts, word);

        } while (PlayAgain());
        
    }
    static void ShowUi(char[] mask, string usedLetters, int attempts)
    {
        Console.Clear();
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.DarkCyan;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Welcome to HANGMAN! ");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine(string.Join(' ', mask));
        Console.WriteLine();
        Console.WriteLine($"Used letters: {usedLetters}");
        Console.WriteLine($"Attempts left: {attempts}");
        Console.WriteLine();
    }
    static void RunGame(char[] mask, string usedLetters, int attempts, string word)
    {
        while (true)
        {
            ShowUi(mask, usedLetters, attempts);

            if (attempts == 0)
            {
                Console.Write($"Hey loser, you lost! The right word was {word}. \n" +
                              "Wanna have an revenge round? [y]es/[n]o: ");
                break;
            }

            if (IsWin(mask))
            {
                Console.Write("Congratz champ, you made it!\n" +
                              "Wanna go for another round? [y]es/[n]o: ");
                break;
            }

            var guessedLetter = CoolReadLetter();

            if (usedLetters.Contains(guessedLetter))
                continue;

            if (!CheckLetter(guessedLetter, word, mask))
                attempts--;

            usedLetters += $"{guessedLetter} ";
        }
    }
    static string[] CreateWordsList()
    {
        var words = new[] 
        {
            "bridle", "horse", "pony", "helmet", "stable",
            "paddock", "dressage", "liberty", "canter", "hindquarter"
        };
        return words;
    }
    static string PickRandomWord(string[] words)
    {
        Random rnd = new Random();
        int random = rnd.Next(words.Length);
        return words[random];
    }
    static bool CheckLetter(char letter, string word, char[] mask)
    {
        var letterFound = false;
        for (int i = 0; i < word.Length; i++)
        {
            if (letter != word[i])
                continue;
            
            mask[i] = letter;
            letterFound = true;
        }

        return letterFound;
    }
    static bool IsWin(char[] mask)
    {
        return !mask.Contains('_');
    }
    static bool PlayAgain()
    { 
        while (true)
        {
            var yesNo = Console.ReadKey(true);

            switch (yesNo.Key)
            {
                case ConsoleKey.Y:
                    return true;
                case ConsoleKey.N:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Good bye.. don't forget to come back soon.");
                    Console.ResetColor();
                    return false;
            }
        }
    }
    static char[] InitMask(int length)
    {
        var mask = new char[length];
        Array.Fill(mask, '_');
        return mask;
    }
    static char CoolReadLetter(params char[] accept)
    {
        char letter = default;

        Console.Write($"Your guess: ");
        
        while (true)
        {
            
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter && letter != default)
            {
                Console.WriteLine();
                return char.ToLower(letter);
            }

            if (key.Key == ConsoleKey.Backspace && letter != default)
            {
                letter = default;
                Console.Write("\b \b");
                continue;
            }
            if (!char.IsLetter(key.KeyChar) || letter != default)
                continue;
            if (accept.Length == 0 || accept.Contains(key.KeyChar))
            {
                letter = key.KeyChar;
                Console.Write(key.KeyChar);
            }
            
        }
    }
}