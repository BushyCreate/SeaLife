using Godot;
using System;

public partial class SellSpot : Node3D
{

    [Signal] public delegate void OnSoldEventHandler();
    public void OnBodyEnter(Node3D body)
    {
        if (body is Player)
        {
            Player player = body as Player;
            player.HUDManager.AddMoney(player.HUDManager.Fish * 10);
            player.HUDManager.DeleteFish();
            Sold();
        }
    }






    public void Sold()
    {
        EmitSignal(SignalName.OnSold);
    }
}
