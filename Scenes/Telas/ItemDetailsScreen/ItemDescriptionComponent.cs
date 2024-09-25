using Godot;
using System;

public partial class ItemDescriptionComponent : Control
{
    RichTextLabel label;
    Polygon2D polygon;

    public override void _Ready()
    {
        label = (RichTextLabel)FindChild("RichTextLabel");
        polygon = (Polygon2D)FindChild("Polygon2D");
    }

    public void ExibirMensagem(string text, Color cor)
    {
        label.Text = text;
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", cor);
    }

    public void destacarComponente()
    {
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", Colors.Black);
        polygon.Color = Colors.White;
    }

    public void removerDestaque()
    {
        label.RemoveThemeColorOverride("default_color");
        label.AddThemeColorOverride("default_color", Colors.White);
        polygon.Color = Colors.Black;
    }

}
