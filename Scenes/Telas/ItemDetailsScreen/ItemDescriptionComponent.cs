using Godot;
using System;
using System.Text;

public partial class ItemDescriptionComponent : Control
{
    public StringBuilder currentText {get; private set;} = new("");
    RichTextLabel label;
    Polygon2D polygon;
    BlinkingComponent blinkingRectangle;

    bool writable = false;

    public override void _Ready()
    {
        label = (RichTextLabel)FindChild("RichTextLabel");
        polygon = (Polygon2D)FindChild("Polygon2D");
        blinkingRectangle = (BlinkingComponent)FindChild("BlinkingRectangle");

        //this.SetProcessInput(false);
    }

    public void ExibirMensagem(string text, Color cor)
    {
        label.Text = text;
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", cor);
    }

    public void destacarComponente()
    {
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", Colors.Black);
        polygon.Color = Colors.White;
    }

    public void removerDestaque()
    {
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", Colors.White);
        polygon.Color = Colors.Black;
    }

     public void onAtivarModoDigitacao(string startingText = "")
    {
        if(startingText != string.Empty)
        {
            currentText.Clear();
            currentText.Append(startingText);
        }

        //this.SetProcessInput(true);
        writable = true;

        blinkingRectangle.ativarBlinking();

        label.Text = currentText.ToString();
        label.RemoveThemeColorOverride("default_color");

        polygon.Color = Colors.Black;
    }

    public void onPausarModoDigitacao()
    {
        //this.SetProcessInput(false);
        writable = false;
        blinkingRectangle.desativarBlinking();
    }

    public void onDesativarModoCriarItem()
    {
        //this.SetProcessInput(false);
        writable = false;
        blinkingRectangle.desativarBlinking();
        this.currentText.Clear();
    }

    public void onConfirmacaoEscolhida(bool confirmar)
    {
        if(!confirmar)
        {
            this.onDesativarModoCriarItem();
            label.Text = currentText.ToString();
            label.RemoveThemeColorOverride("default_color");
        }
    }

    public override void _Input(InputEvent @event)
    {

        if(!writable){return;}

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

            this.label.Text = currentText.ToString();
        }
    }


}
