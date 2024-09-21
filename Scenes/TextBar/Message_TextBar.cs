using Godot;
using System;
using System.Text;

public partial class Message_TextBar : Control
{
    StringBuilder currentText = new();
    Label textLabel;

    public override void _Ready()
    {
        textLabel = (Label)FindChild("Label");
        GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler").AtivarModoCriarBucketListItem += this.AtivarModoCriarItem;
        this.SetProcessInput(false);
    }

    public void AtivarModoCriarItem()
    {
        this.SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey input && input.IsReleased())
        {
            StringBuilder inputCode = new();
            inputCode.Append(input.AsTextKeyLabel().ToLower());

            if(inputCode.Equals("backspace") && currentText.Length > 0 )
            {
                currentText.Remove(currentText.Length - 1, 1);
            }
            if(inputCode.Equals("space") && currentText.Length > 0 )
            {
                currentText.Append(' ');
            }

            if(input.ShiftPressed)
            {
                inputCode.Replace("shift+", "");
                inputCode.Replace(inputCode.ToString(), inputCode.ToString().ToUpper());
            }

            if(inputCode.Length == 1)
            {
                currentText.Append(inputCode);
            }

            this.textLabel.Text = currentText.ToString();
        }
    }


}
