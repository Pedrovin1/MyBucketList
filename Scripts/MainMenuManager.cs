using CustomComponents;
using Godot;
using System;



public partial class MainMenuManager : Node
{
    private enum ActionModes
    {
        Off,
        ItemCreation,
        ItemEditting,
        Scrolling,
        TextSearch,
    }

    private MainMenuHandler _handler;
    ActionModes currentMode = ActionModes.Scrolling;

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
        if (this.currentMode == ActionModes.Off) { return; }

        if (@event.IsActionReleased(nameof(EventNames.ArrowUp)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }
            this._handler.MoveSelectionUp();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.ArrowDown)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }
            this._handler.MoveSelectionDown();
            return;
        }
        
        if (@event.IsActionReleased(nameof(EventNames.Tab)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Esc)))
        {
            return;
        }
        if (@event.IsActionReleased(nameof(EventNames.Space)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Enter)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Shift_Tab)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Delete)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Q)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.R)))
        {
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.E)))
        {
            return;
        }
    }
}
