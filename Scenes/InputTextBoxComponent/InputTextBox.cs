using CustomComponents;
using Godot;
using System;

public partial class InputTextBox : TitleTextBox
{
    [Signal]
    public delegate void TextSubmittedEventHandler(string text);


    private LineEdit _inputNode;

    public override void _Ready()
    {
        base._Ready();

        //Nodes
        this._inputNode = this.GetNode<LineEdit>("%InputLine");

        //Signals
        this._inputNode.TextSubmitted += this.onTextSubmitted;

    }

    public void onTextSubmitted(string text) => this.EmitSignal(InputTextBox.SignalName.TextSubmitted, text);

    public void activateKeyboardTextListening()
    {

        this.hideTextLabel();
        this._inputNode.Show();

        this._inputNode.Text = getCurrentTitleText();

        this._inputNode.Editable = true;
        this._inputNode.FocusMode = FocusModeEnum.Click;
        this._inputNode.CallDeferred(LineEdit.MethodName.GrabFocus);
    }

    public void stopKeyboardTextListening(bool saveChanges = false, bool wipeInputText = false)
    {
        this.showTextLabel();
        this._inputNode.Hide();

        if (saveChanges) { this.updateText(this._inputNode.Text); }
        if (wipeInputText) { this.wipeInputText(); }


        this._inputNode.Editable = false;
        this._inputNode.FocusMode = FocusModeEnum.None;
        this._inputNode.CallDeferred(LineEdit.MethodName.ReleaseFocus);
    }

    public void wipeInputText() => this._inputNode.Text = string.Empty;

    public override void changeTextColor(Color color)
    {
        this._inputNode.AddThemeColorOverride("font_uneditable_color", color);
        this._inputNode.AddThemeColorOverride("font_color", color);
        this._inputNode.AddThemeColorOverride("caret_color", color);
        base.changeTextColor(color);
    }
    public override void resetTextColor()
    {
        this._inputNode.RemoveThemeColorOverride("font_uneditable_color");
        this._inputNode.RemoveThemeColorOverride("font_color");
        this._inputNode.RemoveThemeColorOverride("caret_color");
        base.resetTextColor();
    }
}
