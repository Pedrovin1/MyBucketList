using Godot;
using System;
using System.Runtime.InteropServices;

public partial class InputHandler_MainScreen : Node
{
    [Signal]
    public delegate void SelecionarItemSucessorEventHandler(int movimentacao);
    [Signal]
    public delegate void SelecionarItemAntecessorEventHandler(int movimentacao);
    [Signal]
    public delegate void AtivarModoCriarBucketListItemEventHandler();

    private enum ModosManipulacaoLista
    {
        Default,
        Edicao
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
            case "Up": this.EmitSignal(SignalName.SelecionarItemAntecessor, 1); break;
            case "Down": this.EmitSignal(SignalName.SelecionarItemSucessor, 1); break;
            case "Enter": break;
            case "Tab": break;
            case "Space":
                modoManipulacaoAtual = ModosManipulacaoLista.Edicao; 
                this.EmitSignal(SignalName.AtivarModoCriarBucketListItem);
                break;

            default: break;
        }
    }

    
}
