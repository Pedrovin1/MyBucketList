using Godot;
using System;

public partial class ItemDetailsScreen : Control
{
    [Signal]
    public delegate void AtivarMainScreenEventHandler();

    private Message_TextBar titleTextBar;
    private ItemDescriptionComponent itemDescriptionComponent;
    private Message_TextBar bottomTextBar;

    CanvasLayer canvasRoot;
    InputHandler_ItemDetailsScreen inputHandlerComponent;

    BucketItem selectedBucketItem = null;

    public override void _Ready()
    {
        inputHandlerComponent = (InputHandler_ItemDetailsScreen)FindChild("InputHandler");
        inputHandlerComponent.SairTelaAtual += this.onSairTelaAtual;
        inputHandlerComponent.SalvarEdicaoTexto += this.onSalvarEdicao;

        titleTextBar = (Message_TextBar)FindChild("Title");
        itemDescriptionComponent = (ItemDescriptionComponent)FindChild("ItemDescriptionComponent");
        bottomTextBar = (Message_TextBar)FindChild("Message_TextBar");
        GetNode<MainScreen>("/root/Node/MainScreen").AtivarItemDetailsScreen += inicializarTelaDetalhes;

        canvasRoot = (CanvasLayer)FindChild("CanvasLayer");
        canvasRoot.Visible = false;
    }
    public void inicializarTelaDetalhes(int listIndexItem)
    {
        selectedBucketItem = DataManager.bucketItems[listIndexItem];

        titleTextBar.onAtivarModoDigitacao(selectedBucketItem.nome);
        titleTextBar.onPausarModoDigitacao();

        itemDescriptionComponent.onAtivarModoDigitacao(selectedBucketItem.descricao);
        itemDescriptionComponent.onPausarModoDigitacao();

        inputHandlerComponent.Ativar();
        canvasRoot.Visible = true;
    }

    public void onSalvarEdicao()
    {
        selectedBucketItem.atualizarDados(titleTextBar.currentText.ToString(), itemDescriptionComponent.currentText.ToString());
    }

    public void onSairTelaAtual()
    {
        this.EmitSignal(SignalName.AtivarMainScreen);
        canvasRoot.Visible = false;

        titleTextBar.onDesativarModoCriarItem();
        itemDescriptionComponent.onDesativarModoCriarItem();
    }


}
