using System;
using System.Collections.Generic;
using System.Threading;

abstract class MindfulnessActivity
{
    protected string Name;
    protected string Description;
    protected int Duration;

    public MindfulnessActivity(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void StartActivity()
    {
        Console.WriteLine($"\nStarting {Name}...");
        Console.WriteLine(Description);
        Console.Write("Enter duration in seconds: ");
        Duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Prepare to begin...");
        ShowAnimation(3);
        Run();
        Console.WriteLine("\nGood job! Activity completed.");
        Console.WriteLine($"You completed {Name} for {Duration} seconds.");
        ShowAnimation(3);
    }

    protected abstract void Run();

    protected void ShowAnimation(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() : base("Breathing Activity", "This activity will help you relax by guiding you through slow breathing.") {}

    protected override void Run()
    {
        for (int i = 0; i < Duration; i += 6)
        {
            Console.WriteLine("Breathe in...");
            ShowAnimation(3);
            Console.WriteLine("Breathe out...");
            ShowAnimation(3);
        }
    }
}

class ReflectionActivity : MindfulnessActivity
{
    private static readonly List<string> Prompts = new()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static readonly List<string> Questions = new()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What did you learn about yourself through this experience?"
    };

    public ReflectionActivity() : base("Reflection Activity", "This activity helps you reflect on your strengths and resilience.") {}

    protected override void Run()
    {
        Random random = new();
        string prompt = Prompts[random.Next(Prompts.Count)];
        Console.WriteLine(prompt);
        ShowAnimation(3);
        for (int i = 0; i < Duration; i += 5)
        {
            Console.WriteLine(Questions[random.Next(Questions.Count)]);
            ShowAnimation(5);
        }
    }
}

class ListingActivity : MindfulnessActivity
{
    private static readonly List<string> Prompts = new()
    {
        "Who are people that you appreciate?",
        "What are your personal strengths?",
        "Who are people you have helped this week?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() : base("Listing Activity", "This activity helps you reflect on the good things in your life.") {}

    protected override void Run()
    {
        Random random = new();
        string prompt = Prompts[random.Next(Prompts.Count)];
        Console.WriteLine(prompt);
        ShowAnimation(3);
        Console.WriteLine("Start listing items (press Enter after each item, type 'done' to finish):");
        int count = 0;
        DateTime endTime = DateTime.Now.AddSeconds(Duration);
        while (DateTime.Now < endTime)
        {
            string input = Console.ReadLine();
            if (input.ToLower() == "done") break;
            count++;
        }
        Console.WriteLine($"You listed {count} items!");
    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nMindfulness Program");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an activity: ");
            string choice = Console.ReadLine();

            MindfulnessActivity activity = choice switch
            {
                "1" => new BreathingActivity(),
                "2" => new ReflectionActivity(),
                "3" => new ListingActivity(),
                "4" => null,
                _ => throw new Exception("Invalid choice!")
            };

            if (activity == null)
                break;

            activity.StartActivity();
        }
    }
}
