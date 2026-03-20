using Godot;
using System;

public partial class SellSpot : Node3D
{

    [Signal] public delegate void OnSoldEventHandler(); // Signal that detects when something is sold
    public void OnBodyEnter(Node3D body) // When a player touches the SellSpot, sell the fish and give them money.
    {
        if (body is Player)
        {
            Player player = body as Player;
            player.playerStats.AddMoney(player.playerStats.Fish * 10);
            player.playerStats.DeleteFish();
            EmitSignal(SignalName.OnSold);

        }
    }
}