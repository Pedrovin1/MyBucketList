using Godot;
using System;

public partial class BucketListEntry : Control
{
    Polygon2D backgroundPolygon;
    public Label label;

    public override void _Ready()
    {
        backgroundPolygon = (Polygon2D)FindChild("Polygon2D"); 
        label = (Label)FindChild("Label"); 
           
        backgroundPolygon.Color = Colors.Black;
        label.AddThemeColorOverride("font_color", Colors.White);
    }

    public void alterarTexto(string novoTexto)
    {
        label.Text = novoTexto;
    }

    public BucketListEntry selecionarItem()
    {
        backgroundPolygon.Color = Colors.White;
        label.RemoveThemeColorOverride("font_color");
        label.AddThemeColorOverride("font_color", Colors.Black);
        return this;
    }

    public void deselecionarItem()
    {
        backgroundPolygon.Color = Colors.Black;
        label.RemoveThemeColorOverride("font_color");
        label.AddThemeColorOverride("font_color", Colors.White);
    }


}
