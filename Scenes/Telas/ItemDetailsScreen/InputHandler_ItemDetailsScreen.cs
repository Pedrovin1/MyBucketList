using Godot;
using System;

public partial class InputHandler_ItemDetailsScreen : Node
{
    [Signal]
    public delegate void SelecionarItemSucessorEventHandler();
    [Signal]
    public delegate void SelecionarItemAntecessorEventHandler();
    
    [Signal]
    public delegate void AtivarModoEdicaoTextoEventHandler(string startingText);

    [Signal]
    public delegate void SairTelaAtualEventHandler();

    private enum ModosManipulacaoLista
    {
        Default,
        Edicao,
        Confirmacao,
        Inativo
    }

    ModosManipulacaoLista modoManipulacaoAtual = ModosManipulacaoLista.Inativo;
    public override void _Ready()
    {
        GetNode<InputManager>("/root/InputManager").TeclaContextualPressionada += this.onTeclaContextualPressionada;
    }

    public void onTeclaContextualPressionada(InputEvent @event)
    {
        if(modoManipulacaoAtual == ModosManipulacaoLista.Inativo){ return; }

        InputEventKey tecla = @event as InputEventKey;
        switch(tecla.AsTextKeyLabel())
        {
            case "Escape":
                if (modoManipulacaoAtual == ModosManipulacaoLista.Default)
                {
                    //this.EmitSignal(SignalName.)
                    modoManipulacaoAtual = ModosManipulacaoLista.Inativo;
                    this.EmitSignal(SignalName.SairTelaAtual);
                }
                break;
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

    public void Ativar()
    {
        modoManipulacaoAtual = ModosManipulacaoLista.Default;
    }
}
