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
    [Signal]
    public delegate void DesativarModoCriarBucketListItemEventHandler();

    [Signal]
    public delegate void AdicionarNovoBucketItemEventHandler();

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

            case "Enter": 
                switch(modoManipulacaoAtual)
                {
                    case ModosManipulacaoLista.Default: break; //To implement
                    case ModosManipulacaoLista.Edicao:
                        this.EmitSignal(SignalName.AdicionarNovoBucketItem);
                        this.EmitSignal(SignalName.DesativarModoCriarBucketListItem);
                        modoManipulacaoAtual = ModosManipulacaoLista.Default;
                        break;
                }
                break;

            case "Tab": break;
            case "Space":
                modoManipulacaoAtual = ModosManipulacaoLista.Edicao; 
                this.EmitSignal(SignalName.AtivarModoCriarBucketListItem);
                break;

            default: break;
        }
    }

    
}
