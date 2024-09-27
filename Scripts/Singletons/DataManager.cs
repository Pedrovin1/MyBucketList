using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class DataManager : Node
{
    private static string appDataFolderPath;
    private static string appDataJsonPath;

    public static List<BucketItem> bucketItems = new();
    public static Exception errorStatus = null;

    public override void _Ready()
    {
        DataManager.appDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData, System.Environment.SpecialFolderOption.Create);
        DataManager.appDataFolderPath += @"\MyBucketList";
        DataManager.appDataJsonPath = appDataFolderPath + @"\bucketlist_data.json";

        this.ImportarLista(); 
    }

    public static void ExportarLista()
    {
        string jsonString = System.Text.Json.JsonSerializer.Serialize(bucketItems);

        if(!Godot.DirAccess.DirExistsAbsolute(appDataFolderPath))
        {
            DirAccess.MakeDirAbsolute(appDataFolderPath);
        }
        if(!Godot.FileAccess.FileExists(appDataJsonPath))
        {
            File.Create(appDataJsonPath);
        }

        var file = Godot.FileAccess.Open(appDataJsonPath, Godot.FileAccess.ModeFlags.Write);
        file.StoreString(jsonString);
        file.Close();
    }

    private void ImportarLista()
    {
        string jsonString = string.Empty;

        if(!Godot.DirAccess.DirExistsAbsolute(DataManager.appDataFolderPath))
        {
            Godot.DirAccess.MakeDirAbsolute(DataManager.appDataFolderPath);
        }
        if(!Godot.FileAccess.FileExists(DataManager.appDataJsonPath))
        {
            File.Create(DataManager.appDataJsonPath);
        }

        var file = Godot.FileAccess.Open(DataManager.appDataJsonPath, Godot.FileAccess.ModeFlags.Read);
        jsonString = file.GetAsText();
        file.Close();

        try
        {
            var list = System.Text.Json.JsonSerializer.Deserialize<List<BucketItem>>(jsonString);
            DataManager.bucketItems = list ?? new();
        }
        catch(JsonException e)
        {
            DataManager.errorStatus = e;
        }
        
    }



}
