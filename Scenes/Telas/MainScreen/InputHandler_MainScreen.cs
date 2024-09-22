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
    public delegate void AtivarModoConfirmacaoDecisaoEventHandler(string mensagem);
    [Signal]
    public delegate void ConfirmacaoEscolhidaEventHandler(bool confirmar);

    [Signal]
    public delegate void AdicionarNovoBucketItemEventHandler();
    [Signal]
    public delegate void RemoverBucketItemEventHandler();

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
            case "Escape": 
                switch (modoManipulacaoAtual)
                {
                    case ModosManipulacaoLista.Default: break; //salvar e sair da aplicação
                    case ModosManipulacaoLista.Edicao:
                        this.EmitSignal(SignalName.ConfirmacaoEscolhida, false);
                        modoManipulacaoAtual = ModosManipulacaoLista.Default;
                        break;
                    case ModosManipulacaoLista.Confirmacao: 
                        this.EmitSignal(SignalName.ConfirmacaoEscolhida, false);
                    break;
                }
                break;

            case "Up": this.EmitSignal(SignalName.SelecionarItemAntecessor, 1); break;
            case "Down": this.EmitSignal(SignalName.SelecionarItemSucessor, 1); break;

            case "Enter": 
                switch(modoManipulacaoAtual)
                {
                    case ModosManipulacaoLista.Default: break; //acessar detalhes do item
                    case ModosManipulacaoLista.Edicao:
                        this.EmitSignal(SignalName.AdicionarNovoBucketItem);
                        this.EmitSignal(SignalName.DesativarModoCriarBucketListItem);
                        modoManipulacaoAtual = ModosManipulacaoLista.Default;
                        break;
                }
                break;

            case "Tab": 
                if(modoManipulacaoAtual == ModosManipulacaoLista.Default)
                {
                    this.EmitSignal(SignalName.RemoverBucketItem);
                }
                break;
            case "Space":
                modoManipulacaoAtual = ModosManipulacaoLista.Edicao; 
                this.EmitSignal(SignalName.AtivarModoCriarBucketListItem);
                break;

            default: break;
        }
    }

    
}
