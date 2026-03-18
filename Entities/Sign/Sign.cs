using Godot;
using System;
using DialogueManagerRuntime;

public partial class Sign : Node3D
{
	[Export] Resource dialogueFile; // the Dialogue file that is played
	[Export] Player player; // the player reference
	private bool inArea; // checks if the player is in the area.

	// Shows the dialogue from dialogueFile.
	public void ShowDialogue()
	{
		DialogueManager.ShowDialogueBalloon(dialogueFile, "start");
	}

	// Executes every physics tick, detects when a player is within a range of the sign, if yes show the dialogue.
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
