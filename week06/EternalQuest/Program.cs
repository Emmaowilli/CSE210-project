// I have added an option that allow users to edit or remove/delete goal and
//Unlock achievements when reaching milestones.
// Goal.cs (Abstract Base Class)
// Goal.cs (Abstract Base Class)
using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    protected string _name;
    protected int _points;
    protected bool _isCompleted;
    
    public string Name => _name;
    public int Points => _points;
    public bool IsCompleted => _isCompleted;
    
    public Goal(string name, int points)
    {
        _name = name;
        _points = points;
        _isCompleted = false;
    }
    
    public abstract int RecordEvent();
    public abstract string Display();
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }
    
    public override int RecordEvent()
    {
        _isCompleted = true;
        return _points;
    }
    
    public override string Display()
    {
        return $"[{(_isCompleted ? "X" : " ")}] {_name} - {_points} points";
    }
}

class EternalGoal : Goal
{
    private int _streak;
    
    public EternalGoal(string name, int points) : base(name, points) 
    {
        _streak = 0;
    }
    
    public override int RecordEvent()
    {
        _streak++;
        return _points + (_streak * 2); 
    }
    
    public override string Display()
    {
        return $"[∞] {_name} - {_points} points each time (Streak: {_streak})";
    }
}

class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _currentCount;
    private int _bonusPoints;
    
    public ChecklistGoal(string name, int points, int targetCount, int bonusPoints)
        : base(name, points)
    {
        _targetCount = targetCount;
        _currentCount = 0;
        _bonusPoints = bonusPoints;
    }
    
    public override int RecordEvent()
    {
        _currentCount++;
        if (_currentCount >= _targetCount)
        {
            _isCompleted = true;
            return _points + _bonusPoints;
        }
        return _points;
    }
    
    public override string Display()
    {
        return $"[{(_isCompleted ? "X" : " ")}] {_name} - Completed {_currentCount}/{_targetCount} times, {_points} points each, Bonus: {_bonusPoints} points";
    }
}

class QuestTracker
{
    private List<Goal> _goals = new List<Goal>();
    private int _totalScore = 0;
    private int _level = 1;
    private List<string> _achievements = new List<string>();
    
    public void AddGoal(Goal goal)
    {
        _goals.Add(goal);
    }
    
    public void DisplayGoals()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available.");
            return;
        }
        foreach (var goal in _goals)
        {
            Console.WriteLine(goal.Display());
        }
    }
    
    public void RecordGoal(string name)
    {
        foreach (var goal in _goals)
        {
            if (goal.Name == name)
            {
                _totalScore += goal.RecordEvent();
                CheckLevelUp();
                CheckAchievements();
                DisplayScore();
                return;
            }
        }
        Console.WriteLine("Goal not found.");
    }
    
    private void CheckLevelUp()
    {
        if (_totalScore >= _level * 1000)
        {
            _level++;
            Console.WriteLine($"🎉 Congratulations! You leveled up to Level {_level}! 🎉");
        }
    }
    
    private void CheckAchievements()
    {
        if (_totalScore >= 5000 && !_achievements.Contains("Goal Master"))
        {
            _achievements.Add("Goal Master");
            Console.WriteLine("🏆 Achievement Unlocked: Goal Master!");
        }
    }
    
    public void DisplayAchievements()
    {
        Console.WriteLine("🎖 Achievements Unlocked: " + (_achievements.Count > 0 ? string.Join(", ", _achievements) : "None"));
    }
    
    public void DisplayScore()
    {
        Console.WriteLine($"Total Score: {_totalScore}");
        Console.WriteLine($"Current Level: {_level}");
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
            Console.WriteLine("3. Record Event");
            Console.WriteLine("4. View Achievements");
            Console.WriteLine("5. Quit");
            Console.Write("Select an option: ");
            
            string choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Select goal type: 1. Simple 2. Eternal 3. Checklist");
                        string goalType = Console.ReadLine();
                        Console.Write("Enter goal name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter points: ");
                        int points = int.Parse(Console.ReadLine());
                        
                        if (goalType == "1")
                            tracker.AddGoal(new SimpleGoal(name, points));
                        else if (goalType == "2")
                            tracker.AddGoal(new EternalGoal(name, points));
                        else if (goalType == "3")
                        {
                            Console.Write("Enter target count: ");
                            int target = int.Parse(Console.ReadLine());
                            Console.Write("Enter bonus points: ");
                            int bonus = int.Parse(Console.ReadLine());
                            tracker.AddGoal(new ChecklistGoal(name, points, target, bonus));
                        }
                        else
                            Console.WriteLine("Invalid goal type.");
                        break;
                    case "2":
                        tracker.DisplayGoals();
                        break;
                    case "3":
                        Console.Write("Enter goal name: ");
                        tracker.RecordGoal(Console.ReadLine());
                        break;
                    case "4":
                        tracker.DisplayAchievements();
                        break;
                    case "5":
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





 



        

 

 




