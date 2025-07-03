using Godot;
using System;
using CustomComponents;
using Godot.Bridge;

public partial class ItemDescriptionMenuManager : Node
{
    private enum ActionModes
    {
        Off,
        ItemEditing,
        Scrolling,
    }

    private ItemDescriptionMenuHandler _handler;
    ActionModes currentMode = ActionModes.Off;

    public override void _Ready()
    {
        //Nodes
        VBoxContainer _editableItems = GetNode<VBoxContainer>("%EditableItems");
        InputTextBox _titleBox = GetNode<InputTextBox>("%TitleBox");
        InputTextBox _descriptionBox = GetNode<InputTextBox>("%DescriptionBox");
        TitleTextBox _creatingDateBox = GetNode<TitleTextBox>("%CreationDateText");
        InputTextBox _statusBox = GetNode<InputTextBox>("%StatusBox");

        //Signals
        this.GetOwner().GetParent<Main>().ChangeToItemDescriptionScene += this.onChangeToItemDescriptionScene;
        this.GetOwner().GetParent<Main>().ChangeToItemsListScene += this.onChangeToItemsListScene;

        //Handler
        this._handler = new ItemDescriptionMenuHandler
        (
            itemListIndex: -1,
            sceneRoot: this.GetOwner(),

            editableItems: _editableItems,
            titleBox: _titleBox,
            descriptionBox: _descriptionBox,
            creatingDateBox: _creatingDateBox,
            statusBox: _statusBox
        );
    }

    private void onChangeToItemDescriptionScene(int itemListIndex)
    {
        if (itemListIndex <= -1 || itemListIndex >= UserData.Instance.BucketItems.Count)
        {
            Main.Instance.EmitSignal(Main.SignalName.ChangeToItemsListScene);
            return;
        }

        this.currentMode = ActionModes.Scrolling;
        this.GetOwner<CanvasItem>().Show();
        this._handler.CurrentItemListIndex = itemListIndex;

        this._handler.UpdateUI();
    }

    private void onChangeToItemsListScene()
    {
        this.currentMode = ActionModes.Off;
        this.GetOwner<CanvasItem>().Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (this.currentMode == ActionModes.Off) { return; }

        Viewport viewport = GetViewport();

        if (@event.IsActionReleased(nameof(EventNames.ArrowUp)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }
            this._handler.MoveSelectionUp();

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.ArrowDown)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }
            this._handler.MoveSelectionDown();

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Tab)))
        {
            this._handler.UpdateItemIsDoneData();

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Space)))
        {
            if (this.currentMode != ActionModes.Scrolling) { return; }

            if (this._handler.ActivateSelectedInputBox())
            {
                this.currentMode = ActionModes.ItemEditing;
            }

            viewport.SetInputAsHandled();
        }

        if (@event.IsActionReleased(nameof(EventNames.Esc)))
        {

            switch (currentMode)
            {
                case ActionModes.ItemEditing:
                    this.currentMode = ActionModes.Scrolling;
                    this._handler.DeactivateSelectedInputBox(saveChanges: false);
                    break;

                case ActionModes.Scrolling:
                    Main.Instance.EmitSignal(Main.SignalName.ChangeToItemsListScene);
                    break;
            }

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Enter)))
        {
            if (this.currentMode == ActionModes.ItemEditing)
            {
                this._handler.DeactivateSelectedInputBox(saveChanges: true);
                this._handler.UpdateItemTextData();
            }

            this.currentMode = ActionModes.Scrolling;

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Delete)))
        {
            int listIndex = this._handler.CurrentItemListIndex;

            if (this.currentMode != ActionModes.Scrolling || listIndex == -1)
            {
                return;
            }

            this._handler.DeleteBucketItem(listIndex);
            Main.Instance.EmitSignal(Main.SignalName.BucketItemDeleted);

            viewport.SetInputAsHandled();

            Main.Instance.EmitSignal(Main.SignalName.ChangeToItemsListScene);
            return;
        }

        if (@event.IsAction(nameof(EventNames.E)))
        {
            if (@event.IsActionPressed(nameof(EventNames.E)))
            {
                Main.Instance.EmitSignal(Main.SignalName.ChangeKeyInstructionsMenuVisibility, true);
                viewport.SetInputAsHandled();
                return;
            }
            
            if (@event.IsActionReleased(nameof(EventNames.E)))
            {
                Main.Instance.EmitSignal(Main.SignalName.ChangeKeyInstructionsMenuVisibility, false);
                viewport.SetInputAsHandled();
                return;
            }
        }
    }
}

