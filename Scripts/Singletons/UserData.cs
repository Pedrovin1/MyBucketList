using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class UserData : Node
{
    public List<BucketItem> BucketItems { get; private set; }
    private const string UserDataFolderName = "MyBucketList";
    private const string UserDataFileName = "MyBucketListData.json";
    private string UserDataFolderPath;

    public static UserData Instance;

    public override void _Ready()
    {
        UserData.Instance = this;

        this.UserDataFolderPath =
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)
            + @"\" + UserData.UserDataFolderName;

        this.ImportData();
    }

    public void ImportData()
    {
        string saveFilePath = this.UserDataFolderPath + @"\" + UserData.UserDataFileName;

        if (!Godot.DirAccess.DirExistsAbsolute(this.UserDataFolderPath))
        {
            System.IO.Directory.CreateDirectory(this.UserDataFolderPath);
        }
        if (!Godot.FileAccess.FileExists(saveFilePath))
        {
            System.IO.File.Create(saveFilePath).Close();
            this.BucketItems = new();
            return;
        }

        string jsonString;
        using (var file = Godot.FileAccess.Open(saveFilePath, Godot.FileAccess.ModeFlags.Read))
        {
            jsonString = file.GetAsText();
        }

        var parseResult = System.Text.Json.JsonSerializer.Deserialize<List<BucketItem>>(jsonString);
        this.BucketItems = parseResult ?? new();
    }

    public async Task ExportData()
    {
        string jsonString = System.Text.Json.JsonSerializer.Serialize(this.BucketItems);
        string saveFilePath = this.UserDataFolderPath + @"\" + UserData.UserDataFileName;

        int counter = 0;
        bool retry = true;
        const int maxRetries = 5;

        while (counter < maxRetries && retry)
        {
            counter++;
            retry = false;

            try
            {
                if (!Godot.DirAccess.DirExistsAbsolute(this.UserDataFolderPath))
                {
                    System.IO.Directory.CreateDirectory(this.UserDataFolderPath);
                }
                if (!Godot.FileAccess.FileExists(saveFilePath))
                {
                    System.IO.File.Create(saveFilePath).Close();
                }

                using (var file = Godot.FileAccess.Open(saveFilePath, Godot.FileAccess.ModeFlags.Write))
                {
                    file.StoreString(jsonString);
                }
            }
            catch (IOException)
            {
                retry = true;
                await ToSignal(GetTree().CreateTimer(0.3d), Timer.SignalName.Timeout);
            }
        }
    }

    public override async void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            await this.ExportData();
            GetTree().Quit();
        }
    }
}
