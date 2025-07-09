using CustomComponents;
using Godot;
using MyBucketList.Filters;
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
        
        var mainRootNode = this.GetOwner().GetParent<Main>(); 
        mainRootNode.ChangeToItemDescriptionScene += this.onChangeToItemDescriptionScene;
        mainRootNode.ChangeToItemsListScene += this.onChangeToItemsListScene;
        mainRootNode.BucketItemDeleted += this.onBucketItemDeleted;

        //Handler
        this._handler = new MainMenuHandler
        (
            sceneRoot: this.GetOwner(),

            titleBox: _titleBox,
            itemListVBox: _itemList,
            inputBox: _inputBox
        );
    }

    private void onChangeToItemDescriptionScene(int _)
    {
        this.currentMode = ActionModes.Off;
        this.GetOwner<CanvasItem>().Hide();
    }

    private void onChangeToItemsListScene()
    {
        this.currentMode = ActionModes.Scrolling;
        this.GetOwner<CanvasItem>().Show();
        this._handler.SyncChanges();
    }

    private void onBucketItemDeleted()
    {
        this._handler.SyncChanges();
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
            if(this.currentMode != ActionModes.Scrolling){ return; }

            viewport.SetInputAsHandled();

            Main.Instance.EmitSignal(Main.SignalName.ChangeToItemDescriptionScene, this._handler.GetSelectedItem_RootListIndex());
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Esc)))
        {
            if (this.currentMode == ActionModes.Scrolling)
            {
                this.GetTree().Root.PropagateNotification((int)Node.NotificationWMCloseRequest);
            }

            if (this.currentMode == ActionModes.ItemCreation || this.currentMode == ActionModes.ItemEditing)
            {
                this.currentMode = ActionModes.Scrolling;
                this._handler.DeactivateInputBox(wipeText: true);
            }

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Space)))
        {
            if (this.currentMode == ActionModes.Scrolling)
            {
                this.currentMode = ActionModes.ItemCreation;
                this._handler.ActivateInputBox();
            }

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Enter)))
        {
            return;
        }

        if (@event.IsActionPressed(nameof(EventNames.Shift_Tab)))
        {
            if(this.currentMode != ActionModes.Scrolling) { return; }
            this.currentMode = ActionModes.TextSearch;

            this._handler.ActivateInputBox();

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Delete)))
        {
            int listIndex = this._handler.GetSelectedItem_RootListIndex();

            if (this.currentMode != ActionModes.Scrolling || listIndex == -1)
            {
                return;
            }

            this._handler.DeleteBucketItem(listIndex);

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.Q)))
        {
            if (this.currentMode != ActionModes.Scrolling || this._handler.GetSelectedItem_RootListIndex() == -1)
            {
                return;
            }

            this.currentMode = ActionModes.ItemEditing;
            this._handler.ActivateInputBox();

            viewport.SetInputAsHandled();
            return;
        }

        if (@event.IsActionReleased(nameof(EventNames.R)))
        {
            if(this.currentMode != ActionModes.Scrolling){ return; }

            viewport.SetInputAsHandled();

            Random rng = new();
            int randIndex = rng.Next(0, UserData.Instance.BucketItems.Count);

            Main.Instance.EmitSignal(Main.SignalName.ChangeToItemDescriptionScene, randIndex);

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

    private void onTextSubmitted(string inputBoxText)
    {
        switch (this.currentMode)
        {
            case ActionModes.ItemCreation:
                this._handler.AddBucketItem(inputBoxText);
                this._handler.DeactivateInputBox(wipeText:true);

                this._handler.SyncChanges();
                this.currentMode = ActionModes.Scrolling;
            break;

            case ActionModes.ItemEditing:
                int index = this._handler.GetSelectedItem_RootListIndex();

                this._handler.UpdateBucketItemTitle(index, inputBoxText);
                this._handler.DeactivateInputBox(wipeText:true);
                this.currentMode = ActionModes.Scrolling;
            break;

            case ActionModes.TextSearch:

                this._handler.AddFilter( (BucketFilters.Title, inputBoxText).ToTuple<BucketFilters, object>() );

                this._handler.DeactivateInputBox(wipeText:true);
                this._handler.SyncChanges();
                this.currentMode = ActionModes.Scrolling;
            break;

            default: return;
        }
    }
}
