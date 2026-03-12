using Godot;
using System;

public partial class SellSpot : Node3D
{
    public void OnBodyEnter(Node3D body)
    {
        if (body is Player)
        {
            Player player = body as Player;
            player.HUDManager.AddMoney(player.HUDManager.Fish * 10);
            player.HUDManager.DeleteFish();
        }
    }
}
