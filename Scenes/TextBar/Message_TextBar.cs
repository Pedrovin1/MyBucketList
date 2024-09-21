using Godot;
using System;
using System.Text;

public partial class Message_TextBar : Control
{
    public StringBuilder currentText {get; private set;}= new();
    private Label textLabel;

    public override void _Ready()
    {
        textLabel = (Label)FindChild("Label");
        GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler").AtivarModoCriarBucketListItem += this.onAtivarModoCriarItem;
        GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler").DesativarModoCriarBucketListItem += this.onDesativarModoCriarItem;
        this.SetProcessInput(false);
    }

    public void onAtivarModoCriarItem()
    {
        this.SetProcessInput(true);
        textLabel.Text = currentText.ToString();
        textLabel.RemoveThemeColorOverride("font_color");
    }
    public void onDesativarModoCriarItem()
    {
        this.SetProcessInput(false);
        this.currentText.Clear();
    }

    public void ExibirMensagem(string text, Color cor)
    {
        textLabel.RemoveThemeColorOverride("font_color");
        textLabel.AddThemeColorOverride("font_color", cor);
        textLabel.Text = text;
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
