using Godot;
using System;

public partial class MainScreen : Control
{
    [Signal]
    public delegate void AtivarItemDetailsScreenEventHandler(int listItemIndex);

    InputHandler_MainScreen inputHandlerComponent;
    CanvasLayer canvasRoot;


    public override void _Ready()
    {
        inputHandlerComponent = (InputHandler_MainScreen)FindChild("InputHandler");
        inputHandlerComponent.VerDetalhesItemSelecionado += onVerDetalhesItemSelecionado;

        canvasRoot = (CanvasLayer)FindChild("CanvasLayer");
        canvasRoot.Visible = true;

        this.checkSaveImportingError();
    }

    public void _on_item_details_screen_ready()
    {
        GetNode<ItemDetailsScreen>("/root/Node/ItemDetailsScreen").AtivarMainScreen += onAtivar;
    }

    public void onVerDetalhesItemSelecionado()
    {
        BucketEntriesManager selectedEntry = (BucketEntriesManager)FindChild("BucketEntriesManager");
        int listIndexSelectedItem = selectedEntry.getListIndexSelectedItem();

        if(listIndexSelectedItem >= 0 && listIndexSelectedItem < DataManager.bucketItems.Count)
        {
            canvasRoot.Visible = false;
            this.EmitSignal(SignalName.AtivarItemDetailsScreen, listIndexSelectedItem);
        }
        else
        {
            this.onAtivar();
        }
    }

    public void onAtivar()
    {
        canvasRoot.Visible = true;
        ( (BucketEntriesManager)this.FindChild("BucketEntriesManager") ).RefreshUI();
        inputHandlerComponent.Ativar();
    }
    public void Desativar()
    {
        canvasRoot.Visible = false;
    }

    public void checkSaveImportingError()
    {
        if(DataManager.errorStatus == null){ return; }
        
        var messageTextBar = (Message_TextBar)FindChild("Message_TextBar");
        messageTextBar.ExibirMensagem("Import of Save File Failed", Colors.Red);
    }


}
