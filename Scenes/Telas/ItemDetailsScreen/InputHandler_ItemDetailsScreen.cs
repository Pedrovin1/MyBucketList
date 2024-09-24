using Godot;
using System;

public partial class InputHandler_ItemDetailsScreen : Node
{
    [Signal]
    public delegate void SelecionarItemSucessorEventHandler(int movimentacao);
    [Signal]
    public delegate void SelecionarItemAntecessorEventHandler(int movimentacao);

    private enum ModosManipulacaoLista
    {
        Default,
        Edicao,
        Confirmacao
    }

    ModosManipulacaoLista modoManipulacaoAtual = ModosManipulacaoLista.Default;
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
            case "Up": 
                if(modoManipulacaoAtual == ModosManipulacaoLista.Default)
                {
                    this.EmitSignal(SignalName.SelecionarItemAntecessor);
                }
                break;
            case "Down":
                if(modoManipulacaoAtual == ModosManipulacaoLista.Default)
                {
                    this.EmitSignal(SignalName.SelecionarItemSucessor);
                }
                break;
            case "Enter": break;
            case "Tab": break;
            case "Space":break;

            default: break;
        }
    }
}
