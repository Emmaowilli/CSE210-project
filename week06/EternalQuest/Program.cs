// I have added an option that allow users to edit or remove/delete goal and
//Unlock achievements when reaching milestones.
using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    public string Name { get; set; }
    public int Points { get; set; }
    public bool IsCompleted { get; set; }
    
    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
        IsCompleted = false;
    }
    
    public abstract int RecordEvent();
    public abstract string Display();
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }
    
    public override int RecordEvent()
    {
        IsCompleted = true;
        return Points;
    }
    
    public override string Display()
    {
        return $"[{(IsCompleted ? "X" : " ")}] {Name} - {Points} points";
    }
}

class EternalGoal : Goal
{
    public int Streak { get; set; }
    
    public EternalGoal(string name, int points) : base(name, points) 
    {
        Streak = 0;
    }
    
    public override int RecordEvent()
    {
        Streak++;
        return Points + (Streak * 2); 
    }
    
    public override string Display()
    {
        return $"[∞] {Name} - {Points} points each time (Streak: {Streak})";
    }
}

class ChecklistGoal : Goal
{
    public int TargetCount { get; set; }
    public int CurrentCount { get; set; }
    public int BonusPoints { get; set; }
    
    public ChecklistGoal(string name, int points, int targetCount, int bonusPoints)
        : base(name, points)
    {
        TargetCount = targetCount;
        CurrentCount = 0;
        BonusPoints = bonusPoints;
    }
    
    public override int RecordEvent()
    {
        CurrentCount++;
        if (CurrentCount >= TargetCount)
        {
            IsCompleted = true;
            return Points + BonusPoints;
        }
        return Points;
    }
    
    public override string Display()
    {
        return $"[{(IsCompleted ? "X" : " ")}] {Name} - Completed {CurrentCount}/{TargetCount} times, {Points} points each, Bonus: {BonusPoints} points";
    }
}

class QuestTracker
{
    private List<Goal> goals = new List<Goal>();
    private int totalScore = 0;
    private int level = 1;
    private List<string> achievements = new List<string>();
    
    public void AddGoal(Goal goal)
    {
        goals.Add(goal);
    }
    
    public void EditGoal(string name)
    {
        foreach (var goal in goals)
        {
            if (goal.Name == name)
            {
                Console.Write("Enter new goal name: ");
                goal.Name = Console.ReadLine();
                Console.Write("Enter new points: ");
                goal.Points = int.Parse(Console.ReadLine());
                Console.WriteLine("Goal updated!");
                return;
            }
        }
        Console.WriteLine("Goal not found.");
    }
    
    public void DeleteGoal(string name)
    {
        goals.RemoveAll(g => g.Name == name);
        Console.WriteLine("Goal deleted.");
    }
    
    public void RecordGoal(string name)
    {
        foreach (var goal in goals)
        {
            if (goal.Name == name)
            {
                totalScore += goal.RecordEvent();
                CheckLevelUp();
                CheckAchievements();
                DisplayScore();
                break;
            }
        }
    }
    
    private void CheckLevelUp()
    {
        if (totalScore >= level * 1000)
        {
            level++;
            Console.WriteLine($"🎉 Congratulations! You leveled up to Level {level}! 🎉");
        }
    }
    
    private void CheckAchievements()
    {
        if (totalScore >= 5000 && !achievements.Contains("Goal Master"))
        {
            achievements.Add("Goal Master");
            Console.WriteLine("🏆 Achievement Unlocked: Goal Master!");
        }
    }
    
    public void DisplayGoals()
    {
        foreach (var goal in goals)
        {
            Console.WriteLine(goal.Display());
        }
    }
    
    public void DisplayAchievements()
    {
        Console.WriteLine("🎖 Achievements Unlocked: " + (achievements.Count > 0 ? string.Join(", ", achievements) : "None"));
    }
    
    public void DisplayScore()
    {
        Console.WriteLine($"Total Score: {totalScore}");
        Console.WriteLine($"Current Level: {level}");
    }
}

class Program
{
    static void Main()
    {
        QuestTracker tracker = new QuestTracker();
        
        while (true)
        {
            Console.WriteLine("\nMenu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Edit Goal");
            Console.WriteLine("4. Delete Goal");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. View Achievements");
            Console.WriteLine("7. Quit");
            Console.Write("Select an option: ");
            
            string choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter goal name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter points: ");
                        int points = int.Parse(Console.ReadLine());
                        tracker.AddGoal(new SimpleGoal(name, points));
                        break;
                    case "2":
                        tracker.DisplayGoals();
                        break;
                    case "3":
                        Console.Write("Enter goal name to edit: ");
                        tracker.EditGoal(Console.ReadLine());
                        break;
                    case "4":
                        Console.Write("Enter goal name to delete: ");
                        tracker.DeleteGoal(Console.ReadLine());
                        break;
                    case "5":
                        Console.Write("Enter goal name: ");
                        tracker.RecordGoal(Console.ReadLine());
                        break;
                    case "6":
                        tracker.DisplayAchievements();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}



 



        

 

 




