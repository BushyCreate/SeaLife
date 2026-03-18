using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
	public float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export] public float rotationSpeed = 2;
	[Export] public MeshInstance3D body;
	[Export] public PackedScene bobberScene;
	[Export] public Marker3D marker;
	[Export] public HUDManager HUDManager;
	[Export] public PlayerStats playerStats;
	[Export] public Sprite3D caughtSprite;
	[Export] public CanvasLayer canvas;
	[Export] public PackedScene popUpScene;
	[Export] public PackedScene leaderboardScene;
	private bool bobberOut;
	private Bobber bobber;
	private Control currentPopUp;
	private Timer timer;
	private Vector3 spawnLocation;

	Vector3 lastdirection = Vector3.Forward;

	public override void _Ready()
	{
		spawnLocation = GlobalPosition;
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (bobberOut) { Speed = 0; } else { Speed = 5.0f; } // Makes it so you cant move when the bobber is out.
		if (GlobalPosition.Y < 0) { GlobalPosition = spawnLocation; } // If you fall in the water it respawns you.
																	  // Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			lastdirection = direction;
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		// Rotates the body towards where you're headed.
		var bodyrot = body.Rotation;
		bodyrot.Y = Mathf.LerpAngle(bodyrot.Y, Mathf.Atan2(-lastdirection.X, -lastdirection.Z), (float)delta * rotationSpeed);
		body.Rotation = bodyrot;

		Velocity = velocity;
		MoveAndSlide();

		if (bobberOut)
		{
			HandleRod();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("LMB"))
		{

			if (bobberOut == false) // Create a bobber
			{
				bobber = bobberScene.Instantiate<Bobber>();
				GetTree().Root.AddChild(bobber);
				bobber.GlobalPosition = marker.GlobalPosition;
				bobber.Rotation = body.Rotation;
				bobberOut = true;
			}
			else // Kill the bobber
			{
				bobber.QueueFree();
				bobberOut = false;
			}

		}
		else if (@event.IsActionPressed("Leaderboard"))
		{
			var Leaderboard = leaderboardScene.Instantiate<Control>();
			canvas.AddChild(Leaderboard);
		}
	}
	private void HandleRod()
	{
		if (bobber.caught && bobber.fish != null) // If something is catched show a popup.
		{
			bobberOut = false;
			CreatePopUp(bobber.fish);
			OnFishCaught();
		}
	}
	private void OnFishCaught() // Kill the bobber and add a fish.
	{
		GD.Print("Caught a fish!");
		bobber.QueueFree();
		playerStats.AddFish();
		bobberOut = false;
	}

	private void CreatePopUp(FishBase fish)
	{
		currentPopUp = popUpScene.Instantiate<Control>();
		canvas.AddChild(currentPopUp);
		var Texture = currentPopUp.GetChild<TextureRect>(3);
		var Title = Texture.GetChild<Label>(0);
		var Description = Texture.GetChild<Label>(1);
		Texture.Texture = fish.Icon;
		Title.Text = fish.Name;
		Description.Text = fish.Description;

		// Create a timer so the popup disappears after a few seconds.
		timer = new Timer();
		GetTree().Root.AddChild(timer);
		timer.WaitTime = 3f;
		timer.OneShot = true;
		timer.Start();
		timer.Timeout += RemovePopUp;
	}
	private void RemovePopUp()
	{
		currentPopUp.QueueFree();
		timer.QueueFree();
	}
}

