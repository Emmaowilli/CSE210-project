using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Video video1 = new Video("Exploring the World", "Emmanuel Owilli", 120);
        Video video2 = new Video("Programming 101", "Albert John", 150);
        Video video3 = new Video("Cooking for Beginners", "James", 200);

        video1.AddComment(new Comment("Emmanuel Dickson", "Great video! Loved the content."));
        video1.AddComment(new Comment("Apio Sharon ", "Informative and well-presented."));
        video1.AddComment(new Comment("Okello Godwill", "Could have used more examples."));

        video2.AddComment(new Comment("Robert", "Nice explanation of loops."));
        video2.AddComment(new Comment("Everline", "I learned a lot from this tutorial."));

        video3.AddComment(new Comment("Fedrick", "This recipe was delicious!"));
        video3.AddComment(new Comment("Grace", "I will definitely try this at home."));
        video3.AddComment(new Comment("Hannah Okello", "The instructions were clear and easy to follow."));

        List<Video> videos = new List<Video> { video1, video2, video3 };

        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of comments: {video.GetCommentCount()}");
            video.DisplayComments();
            Console.WriteLine();
        }
    }
}

public class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }

    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } 
    private List<Comment> Comments { get; set; }

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return Comments.Count;
    }

    public void DisplayComments()
    {
        foreach (var comment in Comments)
        {
            Console.WriteLine($"Comment by {comment.Name}: {comment.Text}");
        }
    }
}
