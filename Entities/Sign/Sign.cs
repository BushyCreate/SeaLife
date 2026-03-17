using Godot;
using System;
using DialogueManagerRuntime;

public partial class Sign : Node3D
{
	[Export] Resource dialogueFile;
	[Export] Player player;
	private bool inArea;
	// Called when the node enters the scene tree for the first time.
	public void ShowDialogue()
	{
		DialogueManager.ShowDialogueBalloon(dialogueFile, "start");
	}

	public override void _PhysicsProcess(double delta)
	{
		var distance = GlobalTransform.Origin.DistanceTo(player.GlobalTransform.Origin);

		if (distance < 1.5 && inArea == false)
		{
			inArea = true;
			ShowDialogue();
		}
		else if (distance > 1.5 && inArea == true)
		{
			inArea = false;
		}
	}

}
