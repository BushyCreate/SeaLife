using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export] public float rotationSpeed = 2;
	[Export] public MeshInstance3D body;

	Vector3 lastdirection = Vector3.Forward;

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
}

