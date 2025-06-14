using CustomComponents;
using Godot;
using System;

public partial class MainMenuHandler : Node
{
    private Node _sceneRoot;

    private TitleTextBox _titleBox;
    private VBoxContainer _itemList;
    private InputTextBox _inputBox;

    public MainMenuHandler(Node sceneRoot, TitleTextBox titleBox, VBoxContainer itemList, InputTextBox inputBox)
    {
        this._sceneRoot = sceneRoot;

        this._titleBox = titleBox;
        this._itemList = itemList;
        this._inputBox = inputBox;
    }

    public void temp(){}
}
