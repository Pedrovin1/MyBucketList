using Godot;
using System;
using System.Runtime.InteropServices;

public partial class InputHandler_MainScreen : Node
{

    int vboxIndexFirstVisibleBucketItem = -1;
    int vboxIndexSelectedItem = -1;
    int maxEntriesPerScreen = 6; //mudar dps

    public override void _Ready()
    {
        GetNode<InputManager>("/root/InputManager").TeclaContextualPressionada += this.onTeclaContextualPressionada;
    }

    public void onTeclaContextualPressionada(InputEvent @event)
    {
        InputEventKey tecla = @event as InputEventKey;
        switch(tecla.AsTextKeycode())
        {
            case "Up": alterarItemSelecionado(-1); break;; //need to make an enum
            case "Down": alterarItemSelecionado(1); break;
            case "Enter": break;
            case "Tab": break;
            case "Space": break;

            default: break;
        }
    }

    private void alterarItemSelecionado(int movimentacao = 0)
    {
        var vbox = this.GetNode<VBoxContainer>("../CanvasLayer/VBoxContainer");
        if(vboxIndexSelectedItem >= 1 && vboxIndexSelectedItem <= maxEntriesPerScreen)
        {
            var bucketListEntry_ = vbox.GetChild(vboxIndexSelectedItem);
            var bucketEntryPolygon_ = (Polygon2D)bucketListEntry_.FindChild("Polygon2D"); 
            var bucketEntryLabel_ = (Label)bucketListEntry_.FindChild("Label");

            bucketEntryPolygon_.Color = Colors.Black;
            bucketEntryLabel_.AddThemeColorOverride("font_color", Colors.White);
        }
        
        vboxIndexSelectedItem += movimentacao;

        vboxIndexSelectedItem = vboxIndexSelectedItem < -1 ? -1 : vboxIndexSelectedItem;
        vboxIndexSelectedItem = vboxIndexSelectedItem > maxEntriesPerScreen ? maxEntriesPerScreen : vboxIndexSelectedItem;

        if(vboxIndexSelectedItem >= 1 && vboxIndexSelectedItem <= maxEntriesPerScreen)
        {
            var bucketListEntry = vbox.GetChild(vboxIndexSelectedItem);
            var bucketEntryPolygon = (Polygon2D)bucketListEntry.FindChild("Polygon2D"); 
            var bucketEntryLabel = (Label)bucketListEntry.FindChild("Label");

            bucketEntryPolygon.Color = Colors.White;
            bucketEntryLabel.AddThemeColorOverride("font_color", Colors.Black);
        }
    }

}
