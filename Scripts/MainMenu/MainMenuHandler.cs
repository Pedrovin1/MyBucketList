using CustomComponents;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMenuHandler : Node
{
    private Node _sceneRoot;

    private TitleTextBox _titleBox;
    private VBoxContainer _itemListVbox;
    private InputTextBox _inputBox;

    private int _vboxIndexSelectedItem = -1;
    private List<BucketItem> _currentItemsList;
    private int _listIndexOffset = 0; //startFromIndex

    public MainMenuHandler(Node sceneRoot, TitleTextBox titleBox, VBoxContainer itemListVBox, InputTextBox inputBox)
    {
        this._currentItemsList = UserData.Instance.BucketItems.ToList();
        
        this._sceneRoot = sceneRoot;

        this._titleBox = titleBox;
        this._titleBox.Connect(SignalName.Ready, Callable.From(this.titleBoxSetup), (uint)GodotObject.ConnectFlags.OneShot);

        this._itemListVbox = itemListVBox;
        this._itemListVbox.Connect(SignalName.Ready, Callable.From(this.onLocalListChanged), (uint)GodotObject.ConnectFlags.OneShot);

        this._inputBox = inputBox;
        this._inputBox.Connect(SignalName.Ready, Callable.From(this.inputBoxSetup), (uint)GodotObject.ConnectFlags.OneShot);
    }

    private void titleBoxSetup()
    {
        this._titleBox.changeBorderThickness(1);
        this._titleBox.changeBorderColor(Colors.White);
        this._titleBox.updateText("MyBucketList");
    }
    private void onLocalListChanged()
    {
        
        this.updateItemsListText(this._listIndexOffset);

        // for (int i = 0; i < Math.Clamp(UserData.Instance.BucketItems.Count, 0, this._itemListVbox.GetChildCount()); i++)
        // {
        //     this._itemListVbox.GetChild<TitleTextBox>(i).updateText(UserData.Instance.BucketItems[i].Title);
        // }
    }

    public void ChangeItemsList(IEnumerable<BucketItem> newList)
    {
        this._currentItemsList = newList.ToList();
        this.onLocalListChanged();
    }

    private void inputBoxSetup()
    {
        this._inputBox.changeBorderThickness(1);
        this._inputBox.changeBorderColor(Colors.White);
    }

    public void SyncChanges() //TODO: deal with null references of deleted items
    {
        //O(n²)
        foreach (BucketItem item in this._currentItemsList.ToList())
        {
            if (UserData.Instance.BucketItems.IndexOf(item) == -1)
            {
                this._currentItemsList.Remove(item);
            }
        }

        if (this._currentItemsList.Count > this._itemListVbox.GetChildCount())
            {
                //it can probably be turned into an O(1) operation
                while (this._listIndexOffset + this._itemListVbox.GetChildCount() > this._currentItemsList.Count)
                {
                    this._listIndexOffset--;
                }
            }
            else
            {
                this._listIndexOffset = 0;
            }
        

        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, this._currentItemsList.Count - 1);

        this.updateItemsListText(this._listIndexOffset);
        this.resetAllItemTextBoxesStyles();
        this.highlightSelectedItem();

    }

    public int GetSelectedItem_RootListIndex()
    {
        if (this._currentItemsList.Count <= 0 || this._vboxIndexSelectedItem == -1)
        {
            return -1;
        }

        int localListindex = this._vboxIndexSelectedItem + this._listIndexOffset;
        BucketItem selectedItem = this._currentItemsList[localListindex];

        return UserData.Instance.BucketItems.IndexOf(selectedItem); 
    }

    public void MoveSelectionUp()
    {
        this._vboxIndexSelectedItem--;
        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, this._currentItemsList.Count - 1);
        this.resetAllItemTextBoxesStyles();

        if (this._vboxIndexSelectedItem <= -1)
        {
            if (this._listIndexOffset <= 0)
            {
                this._vboxIndexSelectedItem = -1;
                return;
            }

            this._vboxIndexSelectedItem = 0;
            this._listIndexOffset--;
            this.updateItemsListText(this._listIndexOffset);
        }

        this.highlightSelectedItem();
    }

    public void MoveSelectionDown()
    {
        this._vboxIndexSelectedItem++;
        this._vboxIndexSelectedItem = Math.Clamp(this._vboxIndexSelectedItem, -1, this._currentItemsList.Count - 1);
        this.resetAllItemTextBoxesStyles();

        if (this._vboxIndexSelectedItem >= this._itemListVbox.GetChildCount())
        {
            this._vboxIndexSelectedItem = this._itemListVbox.GetChildCount() - 1;

            if (this._listIndexOffset +  this._itemListVbox.GetChildCount() + 1 <= this._currentItemsList.Count)
            {
                this._listIndexOffset++;
            }

            this.updateItemsListText(this._listIndexOffset);
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

        this.updateItemsListText(this._listIndexOffset);
    }

    public void DeleteBucketItem(int rootListIndex)
    {
        UserData.Instance.BucketItems.RemoveAt(rootListIndex);
        this.SyncChanges();
    }

    public void UpdateBucketItemTitle(int rootListIndex, string text)
    {
        var bucketItem = UserData.Instance.BucketItems[rootListIndex];

        bucketItem.UpdateItemData
        (
            title: text,

            description: bucketItem.Description,
            done: bucketItem.Done
        );

        this.updateItemsListText(this._listIndexOffset);
    }

    private void updateItemsListText(int localListItemStartIndex)
    {
        if (localListItemStartIndex < 0) { throw new IndexOutOfRangeException($"Invalid Starting Index (Negative Index): {localListItemStartIndex}"); }
        if (localListItemStartIndex >= this._currentItemsList.Count) { throw new IndexOutOfRangeException($"Invalid Starting Index (Out of Range): {localListItemStartIndex}"); }

        foreach (TitleTextBox tb in _itemListVbox.GetChildren().Cast<TitleTextBox>())
        {
            tb.updateText( string.Empty );
        }

        if(this._currentItemsList.Count <= 0){ return; }


        int vboxIndexCounter = 0;
        //NOT inclusive
        int maxIndex = Math.Min(localListItemStartIndex + this._itemListVbox.GetChildCount(), this._currentItemsList.Count);

        for (int i = localListItemStartIndex; i < maxIndex; i++)
        {
            var textBox = this._itemListVbox.GetChild<TitleTextBox>(vboxIndexCounter);
            textBox.updateText(this._currentItemsList[i].Title);
            vboxIndexCounter++;
        }
    }

    private void resetAllItemTextBoxesStyles()
    {
        foreach (TitleTextBox tb in _itemListVbox.GetChildren().Cast<TitleTextBox>())
        {
            tb.resetBackgroundColor();
            tb.resetBorderColor();
            tb.resetTextColor();
        }
    }

    private void highlightSelectedItem()
    {
        if(this._vboxIndexSelectedItem <= -1){ return; }

        var textBox = this._itemListVbox.GetChild<TitleTextBox>(this._vboxIndexSelectedItem);
        textBox.changeBackgroundColor(Colors.White);
        textBox.changeTextColor(Colors.Black);
    }
}
