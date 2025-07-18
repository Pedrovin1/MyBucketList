using CustomComponents;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class KeyInstructionsMenu : Node2D
{
    public static class TextFileNames
    {
        public static string ItemsList = "0_ItemListMenu.txt";
        public static string ItemDescription = "1_ItemDescriptionMenu.txt";
    }

    private const string LanguageTranslationsDirectory = "res://Assets/Text/";
    private List<string> _supportedLanguages = new();


    [Export]
    private string _defaultLanguage = "En-Us";

    [Export]
    private string _selectedLanguage = string.Empty;

    private RichTextLabel _label;

    public override void _Ready()
    {
        //Nodes
        this._label = GetNode<RichTextLabel>("%KeyInstructionsText");

        //Signals
        Main mainNode = GetNode<Main>("/root/Main");
             mainNode.ChangeToItemDescriptionScene += this.onChangeToItemDescriptionScene;
             mainNode.ChangeToItemsListScene += this.onChangeToItemsListScene;
             mainNode.ChangeKeyInstructionsMenuVisibility += this.onChangeVisibility;

        //Initializations
        this._supportedLanguages.AddRange(Godot.DirAccess.GetDirectoriesAt(LanguageTranslationsDirectory));

        if (!this._supportedLanguages.Contains(this._selectedLanguage))
        {
            this._selectedLanguage = this._defaultLanguage;
        }

        this.onChangeToItemsListScene(); //To initialize the label text

        //Background Box
        var backgroundBox = this.GetNode<TitleTextBox>("%BackgroundBox");
            backgroundBox.changeBorderThickness(3);
            backgroundBox.changeBorderColor(Colors.White);
            backgroundBox.hideTextLabel();
    }

    private void onChangeVisibility(bool visible)
    {
        this.Visible = visible;
        //this.Visible = !this.Visible;
    }

    private void onChangeToItemDescriptionScene(int _)
    {
        string path =
            LanguageTranslationsDirectory +
            this._selectedLanguage + "/" +
            TextFileNames.ItemDescription;

        string text = Godot.FileAccess.GetFileAsString( path );

        this._label.Text = string.Empty;
        this._label.AddText(text);
    }

    private void onChangeToItemsListScene()
    {
        string path =
            LanguageTranslationsDirectory +
            this._selectedLanguage + "/" +
            TextFileNames.ItemsList;

        string text = Godot.FileAccess.GetFileAsString(path);

        this._label.Text = string.Empty;
        this._label.AddText(text);
    }
}
