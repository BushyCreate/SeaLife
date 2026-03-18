using Godot;
using System;

public partial class SceneChanger : Button
{
	[Export] PackedScene scene;
	public override void _Pressed() // On button press change the scene to the referenced scene
	{
		GetTree().ChangeSceneToPacked(scene);
	}

}
