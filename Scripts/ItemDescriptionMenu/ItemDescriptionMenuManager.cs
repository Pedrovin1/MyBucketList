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
        if (itemListIndex <= -1)
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
                this._handler.DeactivateSelectedInputBox(saveChanges: false);
                return;
            }

            if (this.currentMode == ActionModes.Scrolling)
            {
                Main.Instance.EmitSignal(Main.SignalName.ChangeToItemsListScene);
                return;
            }

            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Enter)))
        {
            if (this.currentMode == ActionModes.ItemEditing)
            {
                this._handler.UpdateItemTextData();
                this._handler.DeactivateSelectedInputBox(saveChanges: true);
            }

            this.currentMode = ActionModes.Scrolling;
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

            Main.Instance.EmitSignal(Main.SignalName.ChangeToItemsListScene);
            return;
        }
    }
}

