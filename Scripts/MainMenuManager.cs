using CustomComponents;
using Godot;
using System;

public partial class MainMenuManager : Node
{
    private TitleTextBox _titleBox;
    private VBoxContainer _itemList;
    private InputTextBox _inputBox;

    private bool active = true;
    public override void _Ready()
    {
        //Nodes
        this._titleBox = GetNode<TitleTextBox>("%TitleBox");
        this._itemList = GetNode<VBoxContainer>("%ItemList");
        this._inputBox = GetNode<InputTextBox>("%InputBox");
    }

    
}
