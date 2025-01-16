//I added three features to exceed the requirements: mood tracking for each journal entry, keyword search functionality to find entries easily
//and a statistics display showing the total entries and mood breakdown.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JournalProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nJournal Program");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display journal");
                Console.WriteLine("3. Save journal to file");
                Console.WriteLine("4. Load journal from file");
                Console.WriteLine("5. Search journal by keyword");
                Console.WriteLine("6. View statistics");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        journal.AddEntry();
                        break;
                    case "2":
                        journal.DisplayEntries();
                        break;
                    case "3":
                        Console.Write("Enter filename to save: ");
                        string saveFile = Console.ReadLine();
                        journal.SaveToFile(saveFile);
                        break;
                    case "4":
                        Console.Write("Enter filename to load: ");
                        string loadFile = Console.ReadLine();
                        journal.LoadFromFile(loadFile);
                        break;
                    case "5":
                        Console.Write("Enter keyword to search: ");
                        string keyword = Console.ReadLine();
                        journal.SearchEntries(keyword);
                        break;
                    case "6":
                        journal.DisplayStatistics();
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }

    class Entry
    {
        public string Date { get; private set; }
        public string Prompt { get; private set; }
        public string Response { get; private set; }
        public string Mood { get; private set; }

        public Entry(string prompt, string response, string mood)
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Prompt = prompt;
            Response = response;
            Mood = mood;
        }

        public override string ToString()
        {
            return $"Date: {Date}\nPrompt: {Prompt}\nResponse: {Response}\nMood: {Mood}\n";
        }

        public string ToFileFormat()
        {
            return $"{Date}|{Prompt}|{Response}|{Mood}";
        }

        public static Entry FromFileFormat(string fileLine)
        {
            string[] parts = fileLine.Split('|');
            return new Entry(parts[1], parts[2], parts[3]) { Date = parts[0] };
        }
    }

    class Journal
    {
        private List<Entry> Entries { get; set; } = new List<Entry>();

        private List<string> Prompts = new List<string>
        {
            "What was the most interesting thing you learned today?",
            "Describe a moment today when you felt truly happy.",
            "What challenge did you face today, and how did you overcome it?",
            "What are you grateful for today?",
            "If you could relive one moment from today, what would it be?"
        };

        public void AddEntry()
        {
            Random random = new Random();
            string prompt = Prompts[random.Next(Prompts.Count)];

            Console.WriteLine(prompt);
            Console.Write("Your response: ");
            string response = Console.ReadLine();

            Console.Write("Rate your mood today (e.g., Happy, Neutral, Sad): ");
            string mood = Console.ReadLine();

            Entries.Add(new Entry(prompt, response, mood));
        }

        public void DisplayEntries()
        {
            if (Entries.Count == 0)
            {
                Console.WriteLine("No entries found.");
                return;
            }

            foreach (var entry in Entries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(new string('-', 30));
            }
        }

        public void SaveToFile(string filename)
        {
            try
            {
                File.WriteAllLines(filename, Entries.Select(e => e.ToFileFormat()));
                Console.WriteLine("Journal saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving journal: {ex.Message}");
            }
        }

        public void LoadFromFile(string filename)
        {
            try
            {
                var lines = File.ReadAllLines(filename);
                Entries = lines.Select(Entry.FromFileFormat).ToList();
                Console.WriteLine("Journal loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading journal: {ex.Message}");
            }
        }

        public void SearchEntries(string keyword)
        {
            var matchingEntries = Entries.Where(e => e.Prompt.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     e.Response.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (!matchingEntries.Any())
            {
                Console.WriteLine("No entries matched your search.");
                return;
            }

            foreach (var entry in matchingEntries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(new string('-', 30));
            }
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("Journal Statistics:");
            Console.WriteLine($"Total entries: {Entries.Count}");

            var moodCounts = Entries.GroupBy(e => e.Mood)
                                     .Select(g => new { Mood = g.Key, Count = g.Count() });

            foreach (var mood in moodCounts)
            {
                Console.WriteLine($"{mood.Mood}: {mood.Count}");
            }
        }
    }
}
