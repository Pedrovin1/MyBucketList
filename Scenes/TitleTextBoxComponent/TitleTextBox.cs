using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CustomComponents;

public partial class TitleTextBox : Control
{
    private Vector2 defaultSize = new Vector2(100f, 50f);

    private Polygon2D _border;
    private Polygon2D _background;
    private RichTextLabel _text;
    private MarginContainer _marginContainer;

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
    }
}
