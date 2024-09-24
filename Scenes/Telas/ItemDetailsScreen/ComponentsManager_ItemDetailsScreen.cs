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
        this.indexSelectedComponent++;
        if(this.indexSelectedComponent > 1){this.indexSelectedComponent = -1;}
        this.selecionarComponente(this.indexSelectedComponent);
    }

    public void onSelecionarItemAntecessor()
    {
        this.indexSelectedComponent--;
        if(this.indexSelectedComponent < -1){this.indexSelectedComponent = -1;}
        this.selecionarComponente(this.indexSelectedComponent);
    }

    public void selecionarComponente(int indexSelectedComponent)
    {
        
    }

}
