using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class BucketEntriesManager : Node
{
    VBoxContainer vboxComponentsList;
    int listIndexFirstVisibleBucketItem = -1;
    int vboxIndexSelectedItem = 0;
    BucketListEntry selectedBucketItem = null;
    int maxEntriesPerScreen = 6;

    List<string> dummyData = new List<string>{"1","2","13951","1233333","mais cosai", "e mais", "e ainda mais","namoral","palceholer"};//,"17999","14545","133","1444","17777","1333","1444","5451","11111","1123"

    public override void _Ready()
    {
        vboxComponentsList = GetNode<VBoxContainer>("../CanvasLayer/VBoxContainer");
        var inputHandlerComponent = GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler");
        inputHandlerComponent.SelecionarItemSucessor += this.onSelecionarItemSucessor;
        inputHandlerComponent.SelecionarItemAntecessor += this.onSelecionarItemAntecessor;
        inputHandlerComponent.AdicionarNovoBucketItem += this.onAdicionarNovoBucketItem;
        inputHandlerComponent.RemoverBucketItem += this.onRemoverBucketItem;

        if(dummyData.Count > 0)
        {
            listIndexFirstVisibleBucketItem = 0;
            this.updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }
    }

    private void onSelecionarItemSucessor(int movimentacao = 0)
    {
        if(dummyData.Count <= 0){ return; }

        if(selectedBucketItem != null )
        {
            selectedBucketItem.deselecionarItem();
            selectedBucketItem = null;
        }

        vboxIndexSelectedItem += movimentacao;
        if(vboxIndexSelectedItem > maxEntriesPerScreen)
        {
            vboxIndexSelectedItem = maxEntriesPerScreen;
            listIndexFirstVisibleBucketItem += movimentacao;

            if(listIndexFirstVisibleBucketItem + maxEntriesPerScreen > dummyData.Count)
            {
                if(dummyData.Count >= maxEntriesPerScreen)
                {
                    listIndexFirstVisibleBucketItem = dummyData.Count - maxEntriesPerScreen;
                }
                else
                {
                    listIndexFirstVisibleBucketItem = 0;
                }
            }

            updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }

        selectedBucketItem = vboxComponentsList.GetChild<BucketListEntry>(vboxIndexSelectedItem);
        selectedBucketItem.selecionarItem();
    }
    private void onSelecionarItemAntecessor(int movimentacao = 0)
    {
        if(dummyData.Count <= 0){ return; }

        if(selectedBucketItem != null )
        {
            selectedBucketItem.deselecionarItem();
            selectedBucketItem = null;
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
            selectedBucketItem = vboxComponentsList.GetChild<BucketListEntry>(vboxIndexSelectedItem);
            selectedBucketItem.selecionarItem();
        }
        
    }

    private void updateVisibleBucketEntries(int listIndexInicial)
    {
        const int OFFSET = 1;
        List<string> newItemsRange;

        BucketListEntry currentBucketItem; //clears the text of all entries, to then update them
        for(int i = 0; i < maxEntriesPerScreen; i++)
        {
            currentBucketItem = vboxComponentsList.GetChild<BucketListEntry>(i + OFFSET);
            currentBucketItem.alterarTexto(string.Empty);
        }

        if(listIndexInicial <= -1)
        {
            if(selectedBucketItem != null )
            {
                selectedBucketItem.deselecionarItem();
                selectedBucketItem = null;
                vboxIndexSelectedItem = 0;
            }
            return;
        }

        if(listIndexInicial + maxEntriesPerScreen > dummyData.Count)
        {
            newItemsRange = dummyData.GetRange(listIndexInicial, dummyData.Count - listIndexInicial);
        }
        else
        {
            newItemsRange = dummyData.GetRange(listIndexInicial, maxEntriesPerScreen);
        }
  
        for(int i = 0; i < newItemsRange.Count; i++)
        {
            currentBucketItem = vboxComponentsList.GetChild<BucketListEntry>(i + OFFSET);
            currentBucketItem.alterarTexto(newItemsRange[i]);
        }
    }

    private void onAdicionarNovoBucketItem()
    {
        Message_TextBar textBar = vboxComponentsList.GetChild<Message_TextBar>(vboxComponentsList.GetChildCount() - 1);
        string textNewBucketItem = textBar.currentText.ToString().Trim();

        textBar.ExibirMensagem("Item Inválido!", Colors.Yellow);

        if(textNewBucketItem != null && textNewBucketItem != string.Empty)
        {
            dummyData.Add(textNewBucketItem);
            textBar.ExibirMensagem("Item Adicionado Com Sucesso!", Colors.Green);

            if(listIndexFirstVisibleBucketItem < 0){ listIndexFirstVisibleBucketItem = 0; }
            updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
        }
    }

    private void onRemoverBucketItem() //to implement: confirmation
    {
        const int OFFSET = 1;
        int listIndexSelectedItem = listIndexFirstVisibleBucketItem + (vboxIndexSelectedItem - OFFSET);

        if(vboxIndexSelectedItem > 0 && listIndexSelectedItem < dummyData.Count)
        {
            dummyData.RemoveAt(listIndexSelectedItem);
            if(this.listIndexFirstVisibleBucketItem >= dummyData.Count)
            {
                this.listIndexFirstVisibleBucketItem = dummyData.Count - 1;
            }
        }

        this.updateVisibleBucketEntries(this.listIndexFirstVisibleBucketItem);
    }


    public void _on_title_ready()
    {
        var titleItem = GetNode<BucketListEntry>("/root/MainScreen/%Title");

        titleItem.alterarTexto("My Bucket List");
        titleItem.label.HorizontalAlignment = HorizontalAlignment.Center;
        titleItem.label.VerticalAlignment = VerticalAlignment.Center;
    }
}
