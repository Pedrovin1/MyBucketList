using Godot;
using System;
using CustomComponents;

public partial class ItemDescriptionMenuManager : Node
{
    private enum ActionModes
    {
        Off,
        ItemEditing,
        Scrolling,
    }

    private ItemDescriptionMenuHandler _handler;
    ActionModes currentMode = ActionModes.Scrolling;

    public override void _Ready()
    {
        //Nodes
        VBoxContainer _editableItems = GetNode<VBoxContainer>("%EditableItems");
        InputTextBox _titleBox = GetNode<InputTextBox>("%TitleBox");
        InputTextBox _descriptionBox = GetNode<InputTextBox>("%DescriptionBox");
        TitleTextBox _creatingDateBox = GetNode<TitleTextBox>("%CreationDateText");
        InputTextBox _statusBox = GetNode<InputTextBox>("%StatusBox");

        //Signals
        

        //Handler
        this._handler = new ItemDescriptionMenuHandler
        (
            sceneRoot: this.GetOwner(),

            editableItems: _editableItems,
            titleBox: _titleBox,
            descriptionBox: _descriptionBox,
            creatingDateBox: _creatingDateBox,
            statusBox: _statusBox
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
            this._handler.UpdateItemIsDoneData();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Space)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }

            //bufferCurrentText ?
            if (this._handler.ActivateSelectedInputBox())
            {
                this.currentMode = ActionModes.ItemEditing;
            }
        }

        if (@event.IsActionReleased(nameof(EventNames.Esc)))
            {
                if (this.currentMode == ActionModes.ItemEditing)
                {
                    this.currentMode = ActionModes.Scrolling;
                    //loadPreviousText ?
                    this._handler.DeactivateSelectedInputBox(wipeText: false);
                }
                return;
            }

        if (@event.IsActionReleased(nameof(EventNames.Enter)))
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
    }

    private void onTextSubmitted(string inputBoxText)
    {
        switch (this.currentMode)
        {
            default: return;
        }
    }
}

