using Godot;
using System;

public partial class BlinkingComponent : Line2D
{


    public void ativarBlinking()
    {
        this.Visible = true;

        var timer = GetChild<Timer>(0);
        timer.Start();
    }

    public void desativarBlinking()
    {
        var timer = GetChild<Timer>(0);
        timer.Stop();

        this.Visible = false;
    }

    public void _on_timer_timeout()
    {
        this.Visible = !this.Visible;
    }
}
