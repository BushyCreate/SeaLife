using Godot;
using System;
using System.Collections.Generic;

public partial class FishingSpot : Node3D
{

	// The maximum stock this fishing spot can have
	[Export] int MaxStock = 3;
	// the Current stock this fishing spot has
	public int CurrentStock;

	// Timers handling time related stuff
	[Export] Timer timer; // The restock timer
	[Export] Timer catchTime; // The time left until ! appears
	[Export] Timer fishTime; // The time left until the fish flees
	private bool fishFound; // Is a fish found
	GpuParticles3D particles; // The spot particles
	[Export] GpuParticles3D particlesCaught; // Played when a fish is caught.
	[Export] FishBase[] CatchableFish; // A array with all the catchable fish in it
	[Export] AudioStreamPlayer idleSound; // Sound that plays when a bobber is in the fishing spot
	[Export] AudioStreamPlayer foundSound; // Sound that plays when a fish is found.

	[Signal] public delegate void CaughtEventEventHandler(); // Signal event that's called when a fish is caught

	private Bobber bob; // The bobber that's touching the spot

	public FishingSpot()
	{
		CurrentStock = MaxStock; // Set the current stock to the max stock
	}

	public override void _Ready()
	{
		particles = FindChild("SpotParticles") as GpuParticles3D;
	}

	public void Restock() // Start a timer and hide the particles until it's done
	{
		GD.Print("Restocking");
		timer.Start();
		particles.Emitting = false;
	}

	public void _on_timer_timeout() // Show the spot again
	{
		CurrentStock = MaxStock;
		particles.Emitting = true;
	}

	public void _on_body_entered(Node3D body) // Detects when a bobber is in the fishing spot
	{
		if (body is Bobber)
		{
			bob = body as Bobber;
			Random rnd = new Random();
			catchTime.WaitTime = rnd.Next(3, 7); // Gets a random wait time between 3 and 7 seconds.
			catchTime.Start();
			idleSound.Play();
		}
	}

	public void FishFound() // When a fish is found, show the ! and start the fishing timer
	{
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = true;
		fishTime.Start();
		fishFound = true;
		foundSound.Play();
		particlesCaught.Emitting = true;
	}
	public override void _Input(InputEvent @event) // Detects input, if theres a fish available it will catch it otherwise it'll cause the fish to flee.
	{
		if (@event.IsActionPressed("RMB") && fishFound == true && CurrentStock != 0)
		{
			OnCaught();
		}
		else if (@event.IsActionPressed("RMB") && fishFound == false)
		{
			FishFled();
		}
	}

	public void FishFled() // The fish fled :( 
	{
		fishFound = false;
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = false;
		CurrentStock--;
		if (CurrentStock == 0) // if no stock left restock.
		{
			Restock();
		}
	}


	public void OnCaught() // Executes when the fish is caught/
	{
		bob.caught = true;
		Random rnd = new Random();
		bob.fish = CatchableFish[rnd.Next(0, CatchableFish.Length - 1)]; // Catch a random fish from the available fish.
		fishFound = false;
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		player.caughtSprite.Visible = false;
		CurrentStock--;
		if (CurrentStock == 0) // if no stock left restock.
		{
			Restock();
		}
		EmitSignal(SignalName.CaughtEvent);
	}
}
