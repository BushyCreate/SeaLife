using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export] public float rotationSpeed = 2;
	[Export] public MeshInstance3D body;
	[Export] public PackedScene bobberScene;
	[Export] public Path3D path;
	[Export] public PathFollow3D pathFollow;
	[Export] public Marker3D marker;

	private bool bobberOut;
	private Bobber bobber;

	Vector3 lastdirection = Vector3.Forward;

	Tween tween;

	public override void _Ready()
	{
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
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
		var bodyrot = body.Rotation;
		bodyrot.Y = Mathf.LerpAngle(bodyrot.Y, Mathf.Atan2(-lastdirection.X, -lastdirection.Z), (float)delta * rotationSpeed);
		body.Rotation = bodyrot;
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("LMB"))
		{

			if (bobberOut == false)
			{
				bobber = bobberScene.Instantiate<Bobber>();
				GetTree().Root.AddChild(bobber);
				bobber.GlobalPosition = marker.GlobalPosition;
				bobber.Rotation = body.Rotation;
				bobberOut = true;
			}
			else
			{
				bobber.QueueFree();
				bobberOut = false;
			}

		}
	}

	private Vector3 GetMouseWorldPosition(Vector2 mouse)
	{
		var space = GetWorld3D().DirectSpaceState;
		var start = GetViewport().GetCamera3D().ProjectRayOrigin(mouse);
		var end = GetViewport().GetCamera3D().ProjectPosition(mouse, 1000);
		PhysicsRayQueryParameters3D rayParams = PhysicsRayQueryParameters3D.Create(from: start, to: end);
		var result = space.IntersectRay(rayParams);
		if (!result.ContainsKey("position")) { return Vector3.Zero; }
		Vector3 pos = (Vector3)result["position"];
		return pos;
	}


	private void SetPath()
	{
		var middle = (GlobalPosition + GetMouseWorldPosition(GetViewport().GetMousePosition())) / 2;
		path.Curve.SetPointPosition(1, new Vector3(middle.X, middle.Y += 2, middle.Z));
		path.Curve.SetPointPosition(2, GetMouseWorldPosition(GetViewport().GetMousePosition()));
		GD.Print(path.Curve.GetBakedPoints()[2]);
	}

}

