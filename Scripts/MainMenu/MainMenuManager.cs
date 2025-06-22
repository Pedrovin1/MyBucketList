using CustomComponents;
using Godot;
using System;



public partial class MainMenuManager : Node
{
    private enum ActionModes
    {
        Off,
        ItemCreation,
        ItemEditing,
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

        //Signals
        _inputBox.TextSubmitted += this.onTextSubmitted;

        //Handler
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
            if (this.currentMode == ActionModes.ItemCreation || this.currentMode == ActionModes.ItemEditing)
            {
                this.currentMode = ActionModes.Scrolling;
                this._handler.DeactivateInputBox(wipeText: true);
            }
            return;
        }
        if (@event.IsActionReleased(nameof(EventNames.Space)))
        {
            if (this.currentMode == ActionModes.Scrolling)
            {
                this.currentMode = ActionModes.ItemCreation;
                this._handler.ActivateInputBox();
            }
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
            int listIndex = this._handler.GetSelectedItemListIndex();

            if (this.currentMode != ActionModes.Scrolling || listIndex == -1)
            {
                return;
            }

            this._handler.DeleteBucketItem(listIndex);

            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Q)))
        {
            if (this.currentMode != ActionModes.Scrolling || this._handler.GetSelectedItemListIndex() == -1)
            {
                return;
            }

            this.currentMode = ActionModes.ItemEditing;
            this._handler.ActivateInputBox();
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

    private void onTextSubmitted(string inputBoxText)
    {
        switch (this.currentMode)
        {
            case ActionModes.ItemCreation:
                this._handler.AddBucketItem(inputBoxText);
                this._handler.DeactivateInputBox(wipeText:true);
                this.currentMode = ActionModes.Scrolling;
            break;

            case ActionModes.ItemEditing:
                int index = this._handler.GetSelectedItemListIndex();

                this._handler.UpdateBucketItemTitle(index, inputBoxText);
                this._handler.DeactivateInputBox(wipeText:true);
                this.currentMode = ActionModes.Scrolling;
            break;

            default: return;
        }
    }
}
