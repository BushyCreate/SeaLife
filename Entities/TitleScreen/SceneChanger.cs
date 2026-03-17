using Godot;
using System;

public partial class SceneChanger : Button
{
	[Export] PackedScene scene;
	// Called when the node enters the scene tree for the first time.
	public override void _Pressed()
	{
		GetTree().ChangeSceneToPacked(scene);
	}

}
