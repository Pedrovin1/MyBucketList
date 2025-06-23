using Godot;
using System;
using CustomComponents;
using System.Linq;

public class ItemDescriptionMenuHandler
{
    public int CurrentItemListIndex {get; set;} = -1;
    public string TextBuffer { get; private set; } = string.Empty;

    private Node _sceneRoot;
    private VBoxContainer _editableItems;
    private InputTextBox _titleBox;
    private InputTextBox _descriptionBox;
    private TitleTextBox _creatingDateBox;
    private InputTextBox _statusBox;

    private int _vboxEditableIndexSelectedItem = -1;
    

    public ItemDescriptionMenuHandler(int itemListIndex, Node sceneRoot, VBoxContainer editableItems, InputTextBox titleBox, InputTextBox descriptionBox, TitleTextBox creatingDateBox, InputTextBox statusBox)
    {
        this.CurrentItemListIndex = itemListIndex;

        this._sceneRoot = sceneRoot;
        this._editableItems = editableItems;
        this._titleBox = titleBox;
        this._descriptionBox = descriptionBox;
        this._creatingDateBox = creatingDateBox;
        this._statusBox = statusBox;
    }

    public void UpdateUI()
    {
        BucketItem item = UserData.Instance.BucketItems[this.CurrentItemListIndex];

        this._titleBox.updateText(item.Title);
        this._descriptionBox.updateText(item.Description);
        this._creatingDateBox.updateText(item.CreatedDate.ToString());

        //temp
        string status = item.Done ? "Done" : "To do";
        this._statusBox.updateText(status);
    }

    public void MoveSelectionUp()
    {
        this._vboxEditableIndexSelectedItem = Math.Max(this._vboxEditableIndexSelectedItem - 1, -1);
        this.highlightSelectedItem();
    }

    public void MoveSelectionDown()
    {
        this._vboxEditableIndexSelectedItem = Math.Min(this._vboxEditableIndexSelectedItem + 1, this._editableItems.GetChildCount() - 1);
        this.highlightSelectedItem();
    }

    public bool ActivateSelectedInputBox()
    {
        if (this._vboxEditableIndexSelectedItem <= -1) { return false; }

        this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem).activateKeyboardTextListening();
        return true;
    }

    public void SaveSelectedInputBoxTextToBuffer()
    {
        this.TextBuffer = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem).getCurrentInputText();
    }

    public void DeactivateSelectedInputBox(bool loadBuffer = false)
    {
        var inputBox = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem);

        if (loadBuffer) { inputBox.updateText(this.TextBuffer); }
        inputBox.stopKeyboardTextListening();
    }

    public void UpdateItemTextData()
    {
        var inputBox = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem);
        string text = inputBox.getCurrentInputText();

        BucketItem item = UserData.Instance.BucketItems[this.CurrentItemListIndex];

        switch (inputBox.Name)
        {
            case "TitleBox":
                item.UpdateItemData
                (
                    title: text,
                    description: item.Description,

                    done: item.Done
                );
                break;

            case "DescriptionBox":
                item.UpdateItemData
                (
                    title: item.Title,
                    description: text,

                    done: item.Done
                );
                break;
        }

        this.UpdateUI();
    }

    public void UpdateItemIsDoneData()
    {
        BucketItem item = UserData.Instance.BucketItems[this.CurrentItemListIndex];
        item.UpdateItemData
        (
            title: item.Title,
            description: item.Description,

            done: !item.Done
        );

        this.UpdateUI();
    }

    public void DeleteBucketItem(int listIndex)
    {
        UserData.Instance.BucketItems.RemoveAt(listIndex);
    }

    private void highlightSelectedItem()
    {
        this.resetAllEditableItemTextBoxesStyles();
        if (this._vboxEditableIndexSelectedItem <= -1) { return; }

        var inputBox = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem);
        inputBox.changeTextColor(Colors.Black);
        inputBox.changeBackgroundColor(Colors.White);
    }

    private void resetAllEditableItemTextBoxesStyles()
    {
        foreach (TitleTextBox tb in this._editableItems.GetChildren().Cast<TitleTextBox>())
        {
            tb.resetBackgroundColor();
            tb.resetBorderColor();
            tb.resetTextColor();
        }
    }
    
}