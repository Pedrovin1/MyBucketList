using CustomComponents;
using Godot;
using System;
using System.Linq;

public partial class MainMenuHandler : Node
{
    private Node _sceneRoot;

    private TitleTextBox _titleBox;
    private VBoxContainer _itemList;
    private InputTextBox _inputBox;

    private int _vboxIndexSelectedItem = -1;
    private int[] _itemListIndexRange = { 0, 0 }; //(Inclusive - Exclusive)

    public MainMenuHandler(Node sceneRoot, TitleTextBox titleBox, VBoxContainer itemList, InputTextBox inputBox)
    {
        this._sceneRoot = sceneRoot;

        this._titleBox = titleBox;
        this._titleBox.Connect(SignalName.Ready, Callable.From(this.titleBoxSetup), (uint)GodotObject.ConnectFlags.OneShot);

        this._itemList = itemList;
        this._itemList.Connect(SignalName.Ready, Callable.From(this.itemListSetup), (uint)GodotObject.ConnectFlags.OneShot);

        this._inputBox = inputBox;
        this._inputBox.Connect(SignalName.Ready, Callable.From(this.inputBoxSetup), (uint)GodotObject.ConnectFlags.OneShot);

        this._itemListIndexRange[1] = Math.Clamp(UserData.Instance.BucketItems.Count(), 0, this._itemList.GetChildCount() );
    }

    private void titleBoxSetup()
    {
        this._titleBox.changeBorderThickness(1);
        this._titleBox.changeBorderColor(Colors.White);
        this._titleBox.updateText("MyBucketList");
    }
    private void itemListSetup()
    {
        for (int i = 0; i < Math.Clamp(UserData.Instance.BucketItems.Count, 0, this._itemList.GetChildCount()); i++)
        {
            this._itemList.GetChild<TitleTextBox>(i).updateText(UserData.Instance.BucketItems[i].Title);
        }
    }

    private void inputBoxSetup()
    {
        this._inputBox.changeBorderThickness(1);
        this._inputBox.changeBorderColor(Colors.White);
    }

    public void SyncChanges()
    {
        while (this._itemListIndexRange[1] > UserData.Instance.BucketItems.Count)
        {
            this._itemListIndexRange[0]--;
            this._itemListIndexRange[0] = Math.Max(0, this._itemListIndexRange[0]);

            this._itemListIndexRange[1]--;
        }

        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, UserData.Instance.BucketItems.Count - 1);

        this.updateItemsListText(this._itemListIndexRange);
        this.resetAllItemTextBoxesStyles();
        this.highlightSelectedItem();
    }

    public int GetSelectedItemListIndex()
    {
        if (this._itemListIndexRange[1] - this._itemListIndexRange[0] <= 0 ||
           this._vboxIndexSelectedItem == -1)
        {
            return -1;
        }

        return this._itemListIndexRange[0] + this._vboxIndexSelectedItem;

    }

    public void MoveSelectionUp()
    {
        this._vboxIndexSelectedItem--;
        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, UserData.Instance.BucketItems.Count - 1);
        this.resetAllItemTextBoxesStyles();

        if (this._vboxIndexSelectedItem <= -1)
        {
            if (this._itemListIndexRange[0] <= 0)
            {
                this._vboxIndexSelectedItem = -1;
                return;
            }

            this._vboxIndexSelectedItem = 0;
            this._itemListIndexRange[0]--;
            this._itemListIndexRange[1]--;
            this.updateItemsListText(this._itemListIndexRange);
        }

        this.highlightSelectedItem();
    }

    public void MoveSelectionDown()
    {
        this._vboxIndexSelectedItem++;
        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, UserData.Instance.BucketItems.Count - 1);
        this.resetAllItemTextBoxesStyles();

        if (this._vboxIndexSelectedItem >= this._itemList.GetChildCount())
        {
            this._vboxIndexSelectedItem = this._itemList.GetChildCount() - 1;

            if (this._itemListIndexRange[1] + 1 <= UserData.Instance.BucketItems.Count)
            {
                this._itemListIndexRange[0]++;
                this._itemListIndexRange[1]++;
            }

            this.updateItemsListText(this._itemListIndexRange);
        }

        this.highlightSelectedItem();
    }

    public void ActivateInputBox()
    {
        this._inputBox.activateKeyboardTextListening();
    }
    public void DeactivateInputBox(bool wipeText = true)
    {
        this._inputBox.stopKeyboardTextListening(saveChanges: false, wipeInputText: wipeText);
    }

    public void AddBucketItem(string titleText)
    {
        UserData.Instance.BucketItems.Add(new BucketItem(titleText));

        if (this._itemListIndexRange[1] < this._itemList.GetChildCount())
        {
            this._itemListIndexRange[1]++;
            this.updateItemsListText(this._itemListIndexRange);
        }
    }

    public void DeleteBucketItem(int listIndex)
    {
        UserData.Instance.BucketItems.RemoveAt(listIndex);
        this.SyncChanges();

        // if (this._itemListIndexRange[1] > UserData.Instance.BucketItems.Count)
        // {
        //     this._itemListIndexRange[1]--;
        //     this.updateItemsListText(this._itemListIndexRange);

        //     if (this._vboxIndexSelectedItem >= this._itemListIndexRange[1])
        //     {
        //         this.MoveSelectionUp();
        //     }
        // }
    }

    public void UpdateBucketItemTitle(int listIndex, string text)
    {
        var bucketItem = UserData.Instance.BucketItems[listIndex];

        bucketItem.UpdateItemData
        (
            title: text,

            description: bucketItem.Description,
            done: bucketItem.Done
        );

        this.updateItemsListText(this._itemListIndexRange);
    }

    private void updateItemsListText(int[] range)
    {
        if (range.Length != 2) { throw new ArgumentException($"Invalid Range, Range Array must have exactly 2 numbers"); }

        int rangeDifference = range[1] - range[0];
        if (rangeDifference > this._itemList.GetChildCount() || rangeDifference > UserData.Instance.BucketItems.Count || rangeDifference < 0)
        {
            throw new ArgumentException($"Invalid Range Values, Range Difference = {rangeDifference}");
        }

        foreach (TitleTextBox tb in _itemList.GetChildren().Cast<TitleTextBox>())
        {
            tb.updateText( string.Empty );
        }

        int vboxIndexCounter = 0;
        for (int i = range[0]; i < range[1]; i++)
        {
            var textBox = this._itemList.GetChild<TitleTextBox>(vboxIndexCounter);
            textBox.updateText(UserData.Instance.BucketItems[i].Title);
            vboxIndexCounter++;
        }
    }

    private void resetAllItemTextBoxesStyles()
    {
        foreach (TitleTextBox tb in _itemList.GetChildren().Cast<TitleTextBox>())
        {
            tb.resetBackgroundColor();
            tb.resetBorderColor();
            tb.resetTextColor();
        }
    }

    private void highlightSelectedItem()
    {
        if(this._vboxIndexSelectedItem <= -1){ return; }

        var textBox = this._itemList.GetChild<TitleTextBox>(this._vboxIndexSelectedItem);
        textBox.changeBackgroundColor(Colors.White);
        textBox.changeTextColor(Colors.Black);
    }
}
