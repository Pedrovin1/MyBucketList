using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CustomComponents;

public partial class TitleTextBox : Control
{
    private Vector2 defaultSize = new Vector2(100f, 50f);
    private const int DefaultMargin = 4;
    

    private Polygon2D _border;
    private Polygon2D _background;
    private RichTextLabel _text;
    private MarginContainer _marginContainer;

    private int _borderThickness = 0;

    public override void _Ready()
    {
        //Nodes
        this._border = this.GetNode<Polygon2D>("%PolygonBorder");
        this._background = this.GetNode<Polygon2D>("%PolygonBackground");
        this._text = this.GetNode<RichTextLabel>("%TextLabel");
        this._marginContainer = this.GetNode<MarginContainer>("%MarginContainer");

        //Signals
        this.Resized += this.onResized;

        if (!this.Size.IsEqualApprox(this.defaultSize))
        {
            this.onResized();
        }
    }

    public void changeBackgroundColor(Color color) => this._background.Color = color;
    public void resetBackgroundColor() => this._background.Color = Colors.Black;

    public void changeBorderColor(Color color) => this._border.Color = color;
    public void resetBorderColor() => this._border.Color = Colors.Black;
    public void changeBorderThickness(int pixelsAmount)
    {
        this._borderThickness = pixelsAmount;

        //to shrink the background polygon2D to make the border polygon2D more visible
        Vector2[] currentBgSize = this._background.Polygon;
        this._background.Polygon = new Vector2[]
        {
            new Vector2(currentBgSize[0].X + pixelsAmount, currentBgSize[0].Y + pixelsAmount),
            new Vector2(currentBgSize[1].X - pixelsAmount, currentBgSize[1].Y + pixelsAmount),
            new Vector2(currentBgSize[2].X - pixelsAmount, currentBgSize[2].Y - pixelsAmount),
            new Vector2(currentBgSize[3].X + pixelsAmount, currentBgSize[3].Y - pixelsAmount),
        };

        this._marginContainer.AddThemeConstantOverride("margin_left", DefaultMargin + pixelsAmount);
        this._marginContainer.AddThemeConstantOverride("margin_right", DefaultMargin + pixelsAmount);
    }

    public void updateText(string newText) => this._text.Text = newText;
    public void showTextLabel() => this._text.Show();
    public void hideTextLabel() => this._text.Hide();
    public void changeTextColor(Color color) => this._text.AddThemeColorOverride("default_color", color);
    public void resetTextColor(Color color) => this._text.RemoveThemeColorOverride("default_color");

    public void onResized()
    {
        float x = this.Size.X;
        float y = this.Size.Y;

        // vertices index
        // 0 ----- 1
        // |       | 
        // 3 ----- 2

        this._border.Polygon = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(x, 0f),
            new Vector2(x, y),
            new Vector2(0f, y)
        };

        this._background.Polygon = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(x, 0f),
            new Vector2(x, y),
            new Vector2(0f, y)
        };

        this.changeBorderThickness(this._borderThickness);
    }
}
