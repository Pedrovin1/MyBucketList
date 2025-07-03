using Godot;
using System;

public partial class Main : Node2D
{
    [Signal]
    public delegate void ChangeToItemDescriptionSceneEventHandler(int itemListIdex);
    [Signal]
    public delegate void ChangeToItemsListSceneEventHandler();

    [Signal]
    public delegate void ChangeKeyInstructionsMenuVisibilityEventHandler(bool visible);

    [Signal]
    public delegate void BucketItemDeletedEventHandler();


    public static Main Instance;

    public override void _Ready()
    {
        Main.Instance = this;
    }

}
