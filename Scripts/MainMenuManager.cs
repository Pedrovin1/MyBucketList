using CustomComponents;
using Godot;
using System;

public partial class MainMenuManager : Node
{
    private MainMenuHandler _handler;

    private bool active = true;
    public override void _Ready()
    {
        //Nodes
        TitleTextBox _titleBox = GetNode<TitleTextBox>("%TitleBox");
        VBoxContainer _itemList = GetNode<VBoxContainer>("%ItemList");
        InputTextBox _inputBox = GetNode<InputTextBox>("%InputBox");

        this._handler = new MainMenuHandler
        (
            sceneRoot: this.GetOwner(),

            titleBox: _titleBox,
            itemList: _itemList,
            inputBox: _inputBox
        );
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        switch (@event.AsText())
        {
            case nameof(EventNames.ArrowUp):
                break;

            case nameof(EventNames.ArrowDown):
                break;

            case nameof(EventNames.Tab):
                break;

            case nameof(EventNames.Esc):
                break;

            case nameof(EventNames.Space):
                break;

            case nameof(EventNames.Enter):
                break;

            case nameof(EventNames.Shift_Tab):
                break;

            case nameof(EventNames.Delete):
                break;

            case nameof(EventNames.Q):
                break;

            case nameof(EventNames.R):
                break;

            case nameof(EventNames.E):
                break;

            default: break;
        }
    }
}
