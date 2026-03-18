using Godot;
using System;

public partial class Upgrader : Node3D
{
	private int Level = 1; // The current playerLevel
	private int Cost = 10; // The money that it takes to upgrade Level.
	[Export] private Label3D label; // The label above the upgrader.
	[Signal] public delegate void OnUpgradedEventHandler(); // Signal that detects when upgraded
	public void OnBodyEntered(Node3D body) // Takes the money from the player if they can afford and make it more expensive to upgrade again.
	{
		if (body is Player)
		{
			Player player = body as Player;
			if (player.playerStats.Money >= Cost)
			{
				player.playerStats.RemoveMoney(Cost);
				player.playerStats.IncreaseLevel();
				Level = player.playerStats.PlayerLevel;
				Cost = Level * 10 + ((Level - 1) * 10);
				label.Text = Cost.ToString() + "$";
				EmitSignal(SignalName.OnUpgraded);
			}
		}
	}
}
