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

    private int _vboxIndexSelectedItem = -1; //-1
    private int[] _itemListIndexRange = { 0, 0 }; //(Inclusive - Exclusive)

    public MainMenuHandler(Node sceneRoot, TitleTextBox titleBox, VBoxContainer itemList, InputTextBox inputBox)
    {
        this._sceneRoot = sceneRoot;

        this._titleBox = titleBox;
        this._titleBox.Connect(SignalName.Ready, Callable.From(this.titleBoxStartUp), (uint)GodotObject.ConnectFlags.OneShot);

        this._itemList = itemList;
        this._itemList.Connect(SignalName.Ready, Callable.From(this.itemListStartUp), (uint)GodotObject.ConnectFlags.OneShot);

        this._inputBox = inputBox;

        this._itemListIndexRange[1] = Math.Clamp(UserData.Instance.BucketItems.Count(), 0, this._itemList.GetChildCount() );
    }

    private void titleBoxStartUp() { this._titleBox.updateText("MyBucketList"); }
    private void itemListStartUp()
    {
        for (int i = 0; i < Math.Clamp(UserData.Instance.BucketItems.Count, 0, this._itemList.GetChildCount()); i++)
        {
            this._itemList.GetChild<TitleTextBox>(i).updateText(UserData.Instance.BucketItems[i].Title);
        }
    }

    public void MoveSelectionUp()
    {
        this._vboxIndexSelectedItem--;
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
        
        this.flagSelectedItem();
    }

    public void MoveSelectionDown()
    {
        this._vboxIndexSelectedItem++;
        this.resetAllItemTextBoxesStyles();

        if (this._vboxIndexSelectedItem >= this._itemList.GetChildCount())
        {
            this._vboxIndexSelectedItem = this._itemList.GetChildCount() - 1;

            if(this._itemListIndexRange[1] + 1 < UserData.Instance.BucketItems.Count)
            {
                this._itemListIndexRange[0]++;
                this._itemListIndexRange[1]++;
            }
            
            this.updateItemsListText(this._itemListIndexRange);
        }

        this.flagSelectedItem();
    }

    private void updateItemsListText(int[] range)
    {
        if (range.Length != 2) { throw new ArgumentException($"Invalid Range, Range Array must have exact 2 numbers"); }

        int rangeDifference = range[1] - range[0];
        if (rangeDifference > this._itemList.GetChildCount() || rangeDifference <= 0)
        {
            throw new ArgumentException($"Invalid Range Values, Range Difference = {rangeDifference}");
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

    private void flagSelectedItem()
    {
        var textBox = this._itemList.GetChild<TitleTextBox>(this._vboxIndexSelectedItem);
        textBox.changeBackgroundColor(Colors.White);
        textBox.changeTextColor(Colors.Black);
    }
}
