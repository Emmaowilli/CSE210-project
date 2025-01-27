// This program helps users memorize scriptures by gradually hiding words.
// Exceeds requirements: Supports a library of scriptures, randomly selects a scripture, and loads scriptures from a file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        ScriptureLibrary scriptureLibrary = new ScriptureLibrary("scriptures.txt");
        Scripture selectedScripture = scriptureLibrary.GetRandomScripture();

        Console.Clear();
        Console.WriteLine("Welcome to the Scripture Memorizer!");
        Console.WriteLine("Press Enter to start memorizing or type 'quit' to exit.");

        string input = Console.ReadLine();
        if (input?.ToLower() == "quit") return;

        while (!selectedScripture.IsFullyHidden())
        {
            Console.Clear();
            Console.WriteLine(selectedScripture.Display());
            Console.WriteLine("\nPress Enter to hide more words or type 'quit' to exit.");

            input = Console.ReadLine();
            if (input?.ToLower() == "quit") break;

            selectedScripture.HideRandomWords();
        }

        Console.Clear();
        Console.WriteLine("Memorization complete! Here is the fully hidden scripture:");
        Console.WriteLine(selectedScripture.Display());
    }
}

class Scripture
{
    private Reference Reference { get; set; }
    private List<Word> Words { get; set; }

    public Scripture(string reference, string text)
    {
        Reference = new Reference(reference);
        Words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public bool IsFullyHidden() => Words.All(word => word.IsHidden);

    public void HideRandomWords()
    {
        Random random = new Random();
        var wordsToHide = Words.Where(word => !word.IsHidden).OrderBy(_ => random.Next()).Take(3);

        foreach (var word in wordsToHide)
        {
            word.Hide();
        }
    }

    public string Display()
    {
        string reference = Reference.ToString();
        string text = string.Join(' ', Words.Select(word => word.Display()));
        return $"{reference}\n{text}";
    }
}

class Reference
{
    public string Book { get; private set; }
    public string StartVerse { get; private set; }
    public string EndVerse { get; private set; }

    public Reference(string reference)
    {
        var parts = reference.Split(' ');
        Book = parts[0];

        var verses = parts[1].Split('-');
        StartVerse = verses[0];
        EndVerse = verses.Length > 1 ? verses[1] : null;
    }

    public override string ToString()
    {
        return EndVerse == null ? $"{Book} {StartVerse}" : $"{Book} {StartVerse}-{EndVerse}";
    }
}

class Word
{
    private string Text { get; set; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public string Display()
    {
        return IsHidden ? new string('_', Text.Length) : Text;
    }
}

class ScriptureLibrary
{
    private List<Scripture> Scriptures { get; set; }

    public ScriptureLibrary(string filePath)
    {
        Scriptures = new List<Scripture>();

        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Scriptures.Add(new Scripture(parts[0].Trim(), parts[1].Trim()));
                }
            }
        }
        else
        {
            Console.WriteLine("Scripture file not found. Using default scripture.");
            Scriptures.Add(new Scripture("John 3:16", "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life."));
        }
    }

    public Scripture GetRandomScripture()
    {
        Random random = new Random();
        return Scriptures[random.Next(Scriptures.Count)];
    }
}
