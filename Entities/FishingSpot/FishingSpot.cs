using Godot;
using System;
using System.Collections.Generic;

public partial class FishingSpot : Node3D
{
	[Export] int MaxStock = 3;
	public int CurrentStock;
	[Export] Timer timer;
	[Export] Timer catchTime;
	[Export] Timer fishTime;
	private bool fishFound;
	[Export] MeshInstance3D mesh;

	[Export] FishBase[] CatchableFish;

	[Signal] public delegate void CaughtEventEventHandler();

	private Bobber bob;

	public FishingSpot()
	{
		CurrentStock = MaxStock;
	}
	public void Restock()
	{
		timer.Start();
		mesh.Visible = false;
	}

	public void _on_timer_timeout()
	{
		CurrentStock = MaxStock;
		mesh.Visible = true;
	}

	public void _on_body_entered(Node3D body)
	{
		if (body is Bobber)
		{
			bob = body as Bobber;
			Random rnd = new Random();
			catchTime.WaitTime = rnd.Next(3, 7);
			catchTime.Start();
		}
	}

	public void FishFound()
	{
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = true;
		fishTime.Start();
		fishFound = true;

	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("RMB") && fishFound == true)
		{
			OnCaught();
		}
	}

	public void FishFled()
	{
		fishFound = false;
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = false;
	}


	public void OnCaught()
	{
		bob.caught = true;
		Random rnd = new Random();
		bob.fish = CatchableFish[rnd.Next(0, CatchableFish.Length - 1)];
		fishFound = false;
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = false;
		CurrentStock--;
		if (CurrentStock == 0)
		{
			Restock();
		}
		EmitSignal(SignalName.CaughtEvent);
	}
}
