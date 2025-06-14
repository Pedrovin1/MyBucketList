using Godot;
using System;

public class BucketItem
{
    //public int ID { get; private set; } = -1; 
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModificationDate { get; set; }
    public bool Done { get; set; } = false;


    public BucketItem(string title = "Untitled")
    {
        //this.ID = 

        this.Title = title;

        this.Description = string.Empty;
        this.CreatedDate = DateTime.Now;
        this.LastModificationDate = DateTime.Now;
        this.Done = false;
    }

    public BucketItem(string title, string description, DateTime createdDate, DateTime lastModified, bool done) //int id, 
    {
        //this.ID = id;
        this.Title = title;
        this.Description = description;
        this.CreatedDate = createdDate;
        this.LastModificationDate = lastModified;
        this.Done = done;
    }

    public void UpdateItemData(string title, string description, bool done)
    {
        this.Title = title;
        this.Description = description;
        this.Done = done;

        this.LastModificationDate = DateTime.Now;
    }
}