using CustomComponents;
using Godot;
using System;

public partial class InputTextBox : TitleTextBox
{
    [Signal]
    public delegate string TextSubmittedEventHandler();


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
        this._inputNode.Editable = true;
        this._inputNode.FocusMode = FocusModeEnum.Click;
        this._inputNode.CallDeferred(LineEdit.MethodName.GrabFocus);
    }

    public void stopKeyboardTextListening()
    {
        this._inputNode.Editable = false;
        this._inputNode.FocusMode = FocusModeEnum.None;
        this._inputNode.CallDeferred(LineEdit.MethodName.ReleaseFocus);
    }

    public void wipeInputText() => this._inputNode.Text = string.Empty;

}
