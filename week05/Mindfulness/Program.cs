// Added: Log file to track activities ,Added: Logging activity completion,// Added: Method to log activity
// Added: To track used questions,// Added: Ensuring all questions are used before repeating,// Added: Collecting user responses
// Added: Displaying number of listed items
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

abstract class MindfulnessActivity
{
    protected string Name;
    protected string Description;
    protected int Duration;
    private static string logFile = "activity_log.txt"; // Log file to track activities

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
        LogActivity(Name, Duration);
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

    private void LogActivity(string activityName, int duration)
    {
        using (StreamWriter writer = new StreamWriter(logFile, true))
        {
            writer.WriteLine($"{DateTime.Now}: Completed {activityName} for {duration} seconds.");
        }
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

    private static List<string> UsedQuestions = new();

    public ReflectionActivity() : base("Reflection Activity", "This activity helps you reflect on your strengths and resilience.") {}

    protected override void Run()
    {
        Random random = new();
        string prompt = Prompts[random.Next(Prompts.Count)];
        Console.WriteLine(prompt);
        ShowAnimation(3);
        for (int i = 0; i < Duration; i += 5)
        {
            if (UsedQuestions.Count == Questions.Count)
                UsedQuestions.Clear();

            string question;
            do
            {
                question = Questions[random.Next(Questions.Count)];
            } while (UsedQuestions.Contains(question));

            UsedQuestions.Add(question);
            Console.WriteLine(question);
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
        Console.WriteLine("Start listing items:");

        List<string> responses = new();
        DateTime endTime = DateTime.Now.AddSeconds(Duration);
        while (DateTime.Now < endTime)
        {
            Console.Write("- ");
            string response = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(response))
                responses.Add(response);
        }
        Console.WriteLine($"You listed {responses.Count} items.");
    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Mindfulness Program!");
            Console.WriteLine("Please select an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. View Activity Log");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();
            MindfulnessActivity activity = null;

            switch (choice)
            {
                case "1":
                    activity = new BreathingActivity();
                    break;
                case "2":
                    activity = new ReflectionActivity();
                    break;
                case "3":
                    activity = new ListingActivity();
                    break;
                case "4":
                    ShowActivityLog();
                    continue;
                case "5":
                    Console.WriteLine("Thank you for using the Mindfulness Program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select again.");
                    Thread.Sleep(2000);
                    continue;
            }

            if (activity != null)
            {
                activity.StartActivity();
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }
    }

    private static void ShowActivityLog()
    {
        string logFile = "activity_log.txt";
        Console.Clear();
        Console.WriteLine("Activity Log:");
        if (File.Exists(logFile))
        {
            string[] logs = File.ReadAllLines(logFile);
            foreach (string log in logs)
            {
                Console.WriteLine(log);
            }
        }
        else
        {
            Console.WriteLine("No activities logged yet.");
        }
        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }
}
