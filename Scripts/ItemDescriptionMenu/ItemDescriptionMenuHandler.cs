using Godot;
using System;
using CustomComponents;
using System.Linq;

public class ItemDescriptionMenuHandler
{
    private Node _sceneRoot;
    private VBoxContainer _editableItems;
    private InputTextBox _titleBox;
    private InputTextBox _descriptionBox;
    private TitleTextBox _creatingDateBox;
    private InputTextBox _statusBox;

    private int _currentItemlistIndex = -1;
    private int _vboxEditableIndexSelectedItem = -1;

    public ItemDescriptionMenuHandler(int itemListIndex, Node sceneRoot, VBoxContainer editableItems, InputTextBox titleBox, InputTextBox descriptionBox, TitleTextBox creatingDateBox, InputTextBox statusBox)
    {
        this._currentItemlistIndex = itemListIndex;

        this._sceneRoot = sceneRoot;
        this._editableItems = editableItems;
        this._titleBox = titleBox;
        this._descriptionBox = descriptionBox;
        this._creatingDateBox = creatingDateBox;
        this._statusBox = statusBox;
    }

    public void MoveSelectionUp()
    {
        this._vboxEditableIndexSelectedItem = Math.Max(this._vboxEditableIndexSelectedItem - 1, -1);
        this.highlightSelectedItem();
    }

    public void MoveSelectionDown()
    {
        this._vboxEditableIndexSelectedItem = Math.Max(this._vboxEditableIndexSelectedItem + 1, this._editableItems.GetChildCount() - 1);
        this.highlightSelectedItem();
    }

    public bool ActivateSelectedInputBox()
    {
        if (this._vboxEditableIndexSelectedItem <= -1) { return false; }

        this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem).activateKeyboardTextListening();
        return true;
    }
    public void DeactivateSelectedInputBox(bool wipeText = false)
    {
        var inputBox = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem);

        if (wipeText) { inputBox.wipeInputText(); }
        inputBox.stopKeyboardTextListening();
    }

    public void UpdateItemTextData(string text)
    {
        var inputBox = this._editableItems.GetChild<InputTextBox>(this._vboxEditableIndexSelectedItem);
        BucketItem item = UserData.Instance.BucketItems[this._currentItemlistIndex];

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
    }

    public void UpdateItemIsDoneData()
    {
        BucketItem item = UserData.Instance.BucketItems[this._currentItemlistIndex];
        item.UpdateItemData
        (
            title: item.Title,
            description: item.Description,

            done: !item.Done
        );
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