using System;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            Console.WriteLine("Welcome to the Guess My Number game!");

            Random random = new Random();
            int magicNumber = random.Next(1, 101);

            int attempts = 0;

            int guess = -1;

            while (guess != magicNumber)
            {
                Console.Write("What is your guess? ");
                string input = Console.ReadLine();

                try
                {
                    guess = int.Parse(input);
                    attempts++; 

                    if (guess < magicNumber)
                    {
                        Console.WriteLine("Higher");
                    }
                    else if (guess > magicNumber)
                    {
                        Console.WriteLine("Lower");
                    }
                    else
                    {
                        Console.WriteLine($"You guessed it! The magic number was {magicNumber}.");
                        Console.WriteLine($"It took you {attempts} attempts.");
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }

            Console.Write("Do you want to play again? (yes/no): ");
            string response = Console.ReadLine().ToLower();

            if (response != "yes")
            {
                playAgain = false;
                Console.WriteLine("Thanks for playing!");
            }
        }
    }
}
