using Godot;
using System;

public partial class SceneChangerSpot : Area3D
{
	[Export] PackedScene scene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += PlayerEntered;
	}

	private void PlayerEntered(Node3D body)
	{
		if (body is Player) { GetTree().ChangeSceneToPacked(scene); }
	}
}
