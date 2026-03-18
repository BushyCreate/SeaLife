using Godot;
using System;

public partial class SceneChangerSpot : Area3D
{
	[Export] PackedScene scene; // The scene that should be showed.
								// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += PlayerEntered; // Add a signal to the BodyEntered function.
	}

	private void PlayerEntered(Node3D body) // Changes the scene to the scene variable
	{
		if (body is Player) { GetTree().ChangeSceneToPacked(scene); }
	}
}
