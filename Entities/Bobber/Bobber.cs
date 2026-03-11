using Godot;
using System;

public partial class Bobber : CharacterBody3D
{
    public bool caught = false;
    public override void _PhysicsProcess(double delta)
    {

        if (!IsOnFloor())
        {
            var forward_dir = -GlobalTransform.Basis.Z;
            Vector3 vel = new Vector3();
            vel.X = Mathf.Clamp(forward_dir.X * 0.5f, -5, 5);
            vel.Z = Mathf.Clamp(forward_dir.Z * 0.5f, -5, 5);
            Velocity += new Vector3(vel.X, GetGravity().Y * (float)delta, vel.Z);
            GD.Print(vel);
        }

        if (IsOnFloor() && Velocity.Y <= 0)
        {
            Velocity = Vector3.Zero;
        }
        MoveAndSlide();
    }
}
