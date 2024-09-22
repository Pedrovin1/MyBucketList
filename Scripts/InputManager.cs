using Godot;
using System;

public partial class InputManager : Node
{
    [Signal]
    public delegate void TeclaContextualPressionadaEventHandler(InputEvent @event);

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("Esc") ||
            @event.IsActionReleased("SetaCima") ||
            @event.IsActionReleased("SetaBaixo") ||
            @event.IsActionReleased("Espaco") ||
            @event.IsActionReleased("Enter") ||
            @event.IsActionReleased("Tab")
        )
        {
            this.EmitSignal(SignalName.TeclaContextualPressionada, @event);
        }
    }
}
