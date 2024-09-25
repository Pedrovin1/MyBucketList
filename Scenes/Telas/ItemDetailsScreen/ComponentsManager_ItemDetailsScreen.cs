using Godot;
using System;

public partial class ComponentsManager_ItemDetailsScreen : Node
{
    private int indexSelectedComponent = -1;
    private Node rootNodeDetailsComponents = null;

    public override void _Ready()
    {
        rootNodeDetailsComponents = GetNode("../CanvasLayer");
    }

    public void onSelecionarItemSucessor()
    {
        this.removerDestaqueComponente(this.indexSelectedComponent);
        this.indexSelectedComponent++;
        if(this.indexSelectedComponent > 1){this.indexSelectedComponent = -1;}
        this.destacarComponente(this.indexSelectedComponent);
    }

    public void onSelecionarItemAntecessor()
    {
        this.removerDestaqueComponente(this.indexSelectedComponent);
        this.indexSelectedComponent--;
        if(this.indexSelectedComponent < -1){this.indexSelectedComponent = -1;}
        this.destacarComponente(this.indexSelectedComponent);
    }

    public void destacarComponente(int indexSelectedComponent)
    {
        if(indexSelectedComponent <= -1) return;
        if(indexSelectedComponent == 0)
        {
            rootNodeDetailsComponents.GetChild<Message_TextBar>(0).destacarComponente();
            return;
        }
        if(indexSelectedComponent == 1)
        {
            //rootNodeDetailsComponents.GetChild<-->(0).destacarComponente();
            return;
        }
    }

    public void removerDestaqueComponente(int indexSelectedComponent)
    {
        if(indexSelectedComponent <= -1) return;
        if(indexSelectedComponent == 0)
        {
            rootNodeDetailsComponents.GetChild<Message_TextBar>(0).removerDestaque();
            return;
        }
        if(indexSelectedComponent == 1)
        {
            //rootNodeDetailsComponents.GetChild<-->(0).destacarComponente();
            return;
        }
    }

}
