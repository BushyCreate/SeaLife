using Godot;
using System;

public partial class FishingSpot : Node3D
{
	[Export] int MaxStock = 3;
	public int CurrentStock;
	[Export] Timer timer;
	[Export] MeshInstance3D mesh;
	// Called when the node enters the scene tree for the first time.

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
			Bobber bob = body as Bobber;
			bob.caught = true;
			CurrentStock--;
		}

	}
}
