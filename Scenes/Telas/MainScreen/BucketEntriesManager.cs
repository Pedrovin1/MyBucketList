using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class BucketEntriesManager : Node
{
    VBoxContainer vboxComponentsList;
    int listIndexFirstVisibleBucketItem = 0;
    int vboxIndexSelectedItem = 0;
    BucketListEntry selectedBucketItem = null;
    int maxEntriesPerScreen = 6; //mudar dps

    List<string> dummyData = new List<string>{"1","2","13951"};//,"17999","14545","133","1444","17777","1333","1444","5451","11111","1123"

    public override void _Ready()
    {
        vboxComponentsList = GetNode<VBoxContainer>("../CanvasLayer/VBoxContainer");
        GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler").SelecionarItemSucessor += this.onSelecionarItemSucessor;
        GetNode<InputHandler_MainScreen>("/root/MainScreen/InputHandler").SelecionarItemAntecessor += this.onSelecionarItemAntecessor;

        this.updateVisibleBucketEntries(listIndexFirstVisibleBucketItem);
    }

    private void onSelecionarItemSucessor(int movimentacao = 0)
    {
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

//implementar verificação de estouro da lista
    private void updateVisibleBucketEntries(int listIndexInicial)
    {
        const int OFFSET = 1;
        List<string> newItemsRange;

        if(dummyData.Count < maxEntriesPerScreen)
        {
            newItemsRange = dummyData.GetRange(listIndexInicial, dummyData.Count);
        }
        else
        {
            newItemsRange = dummyData.GetRange(listIndexInicial, maxEntriesPerScreen);
        }
  
        BucketListEntry currentBucketItem;
        for(int i = 0; i < newItemsRange.Count; i++)
        {
            currentBucketItem = vboxComponentsList.GetChild<BucketListEntry>(i + OFFSET);
            currentBucketItem.alterarTexto(newItemsRange[i]);
        }
    }


    public void _on_title_ready()
    {
        var titleItem = GetNode<BucketListEntry>("/root/MainScreen/%Title");

        titleItem.alterarTexto("My Bucket List");
        titleItem.label.HorizontalAlignment = HorizontalAlignment.Center;
        titleItem.label.VerticalAlignment = VerticalAlignment.Center;
    }
}
