using Godot;
using System;

public partial class InputHandler_ItemDetailsScreen : Node
{
    public override void _Ready()
    {
        GetNode<InputManager>("/root/InputManager").TeclaContextualPressionada += this.onTeclaContextualPressionada;
    }

    public void onTeclaContextualPressionada(InputEvent @event)
    {
        InputEventKey tecla = @event as InputEventKey;
        switch(tecla.AsTextKeyLabel())
        {
            case "Escape":break;
            case "Up": break;
            case "Down":break;
            case "Enter": break;
            case "Tab": break;
            case "Space":break;

            default: break;
        }
    }
}
