using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class BucketEntriesManager : Node
{
    VBoxContainer vboxComponentsList;
    int listIndexFirstVisibleBucketItem = -1;
    int vboxIndexSelectedItem = 0;
    BucketListEntry selectedBucketEntryItem = null;
    int maxEntriesPerScreen = 6;

    public override void _Ready()
    {
        vboxComponentsList = GetNode<VBoxContainer>("../CanvasLayer/VBoxContainer");
        var inputHandlerComponent = GetNode<InputHandler_MainScreen>("/root/Node/MainScreen/InputHandler");
        inputHandlerComponent.SelecionarItemSucessor += this.onSelecionarItemSucessor;
        inputHandlerComponent.SelecionarItemAntecessor += this.onSelecionarItemAntecessor;
        inputHandlerComponent.AdicionarNovoBucketItem += this.onAdicionarNovoBucketItem;
        inputHandlerComponent.RemoverBucketItem += this.onRemoverBucketItem;

        var messageTextBar = GetNode<Message_TextBar>("/root/Node/MainScreen/CanvasLayer/VBoxContainer/Message_TextBar");
        inputHandlerComponent.AtivarModoCriarBucketListItem += messageTextBar.onAtivarModoDigitacao;
        inputHandlerComponent.DesativarModoCriarBucketListItem += messageTextBar.onDesativarModoCriarItem;
        inputHandlerComponent.ConfirmacaoEscolhida += messageTextBar.onConfirmacaoEscolhida;

        if(DataManager.bucketItems.Count > 0)
        {
            listIndexFirstVisibleBucketItem = 0;
            this.updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }
    }

    public int getListIndexSelectedItem()
    {
        return listIndexFirstVisibleBucketItem + vboxIndexSelectedItem - 1;
    }

    private void onSelecionarItemSucessor(int movimentacao = 0)
    {
        if(DataManager.bucketItems.Count <= 0){ return; }

        if(selectedBucketEntryItem != null )
        {
            selectedBucketEntryItem.deselecionarItem();
            selectedBucketEntryItem = null;
        }

        vboxIndexSelectedItem += movimentacao;
        if(vboxIndexSelectedItem > maxEntriesPerScreen)
        {
            vboxIndexSelectedItem = maxEntriesPerScreen;
            listIndexFirstVisibleBucketItem += movimentacao;

            if(listIndexFirstVisibleBucketItem + maxEntriesPerScreen > DataManager.bucketItems.Count)
            {
                if(DataManager.bucketItems.Count >= maxEntriesPerScreen)
                {
                    listIndexFirstVisibleBucketItem = DataManager.bucketItems.Count - maxEntriesPerScreen;
                }
                else
                {
                    listIndexFirstVisibleBucketItem = 0;
                }
            }

            updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }

        selectedBucketEntryItem = vboxComponentsList.GetChild<BucketListEntry>(vboxIndexSelectedItem);
        selectedBucketEntryItem.selecionarItem();
    }
    private void onSelecionarItemAntecessor(int movimentacao = 0)
    {
        if(DataManager.bucketItems.Count <= 0){ return; }

        if(selectedBucketEntryItem != null )
        {
            selectedBucketEntryItem.deselecionarItem();
            selectedBucketEntryItem = null;
        }

        vboxIndexSelectedItem -= movimentacao;
        vboxIndexSelectedItem = vboxIndexSelectedItem < 0 ? 0 : vboxIndexSelectedItem;

        if(vboxIndexSelectedItem < 1 && listIndexFirstVisibleBucketItem - movimentacao >= 0)
        {
            vboxIndexSelectedItem = 1;
            listIndexFirstVisibleBucketItem -= movimentacao;
            updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }
        if(vboxIndexSelectedItem > 0)
        {
            selectedBucketEntryItem = vboxComponentsList.GetChild<BucketListEntry>(vboxIndexSelectedItem);
            selectedBucketEntryItem.selecionarItem();
        }
        
    }

    private void updateVisibleBucketEntries(int listIndexInicial)
    {
        const int OFFSET = 1;
        List<BucketItem> newItemsRange;

        BucketListEntry currentBucketItem; //clears the text of all entries, to then update them
        for(int i = 0; i < maxEntriesPerScreen; i++)
        {
            currentBucketItem = vboxComponentsList.GetChild<BucketListEntry>(i + OFFSET);
            currentBucketItem.alterarTexto(string.Empty);
        }

        if(listIndexInicial <= -1)
        {
            if(selectedBucketEntryItem != null )
            {
                selectedBucketEntryItem.deselecionarItem();
                selectedBucketEntryItem = null;
                vboxIndexSelectedItem = 0;
            }
            return;
        }

        if(listIndexInicial + maxEntriesPerScreen > DataManager.bucketItems.Count)
        {
            newItemsRange = DataManager.bucketItems.GetRange(listIndexInicial, DataManager.bucketItems.Count - listIndexInicial);
        }
        else
        {
            newItemsRange = DataManager.bucketItems.GetRange(listIndexInicial, maxEntriesPerScreen);
        }
  
        for(int i = 0; i < newItemsRange.Count; i++)
        {
            currentBucketItem = vboxComponentsList.GetChild<BucketListEntry>(i + OFFSET);
            currentBucketItem.alterarTexto(newItemsRange[i].nome);
        }
    }

    private void onAdicionarNovoBucketItem()
    {
        Message_TextBar textBar = vboxComponentsList.GetChild<Message_TextBar>(vboxComponentsList.GetChildCount() - 1);
        string textNewBucketItem = textBar.currentText.ToString().Trim();

        textBar.ExibirMensagem("Item Inválido!", Colors.Yellow);

        if(textNewBucketItem != null && textNewBucketItem != string.Empty)
        {
            DataManager.bucketItems.Add(new BucketItem(textNewBucketItem));
            textBar.ExibirMensagem("Item Adicionado Com Sucesso!", Colors.Green);

            if(listIndexFirstVisibleBucketItem < 0){ listIndexFirstVisibleBucketItem = 0; }
            updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }
    }

    private void onRemoverBucketItem() //to implement: confirmation
    {
        const int OFFSET = 1;
        int listIndexSelectedItem = listIndexFirstVisibleBucketItem + (vboxIndexSelectedItem - OFFSET);

        if(vboxIndexSelectedItem > 0 && listIndexSelectedItem < DataManager.bucketItems.Count)
        {
            DataManager.bucketItems.RemoveAt(listIndexSelectedItem);
            if(this.listIndexFirstVisibleBucketItem >= DataManager.bucketItems.Count)
            {
                this.listIndexFirstVisibleBucketItem = DataManager.bucketItems.Count - 1;
            }
        }

        this.updateVisibleBucketEntries(this.listIndexFirstVisibleBucketItem);
    }


    private void _on_title_ready()
    {
        var titleItem = GetNode<BucketListEntry>("/root/Node/MainScreen/%Title");

        titleItem.alterarTexto("My Bucket List");
        titleItem.label.HorizontalAlignment = HorizontalAlignment.Center;
        titleItem.label.VerticalAlignment = VerticalAlignment.Center;
    }
 
    public void RefreshUI()
    {
        this.updateVisibleBucketEntries(this.listIndexFirstVisibleBucketItem);
    }
}
