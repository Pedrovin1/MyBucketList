using Godot;
using System;

public partial class ItemDetailsScreen : Control
{
    [Signal]
    public delegate void AtivarMainScreenEventHandler();

    private Message_TextBar titleTextBar;
    private Message_TextBar bottomTextBar;

    CanvasLayer canvasRoot;
    InputHandler_ItemDetailsScreen inputHandlerComponent;

    private BucketItem bucketItem = null;

    public override void _Ready()
    {
        inputHandlerComponent = (InputHandler_ItemDetailsScreen)FindChild("InputHandler");
        inputHandlerComponent.SairTelaAtual += this.onSairTelaAtual;

        titleTextBar = (Message_TextBar)FindChild("Title");
        bottomTextBar = (Message_TextBar)FindChild("Message_TextBar");
        GetNode<MainScreen>("/root/Node/MainScreen").AtivarItemDetailsScreen += inicializarTelaDetalhes;

        canvasRoot = (CanvasLayer)FindChild("CanvasLayer");
        canvasRoot.Visible = false;
    }
    public void inicializarTelaDetalhes(int listIndexItem)
    {
        BucketItem item = DataManager.bucketItems[listIndexItem];
        titleTextBar.ExibirMensagem(item.nome, Colors.White);
        //inicializar componente descrição
        inputHandlerComponent.Ativar();
        canvasRoot.Visible = true;
    }

    public void onSairTelaAtual()
    {
        this.EmitSignal(SignalName.AtivarMainScreen);
        canvasRoot.Visible = false;
    }


}
