using Godot;
using System;

public partial class Bobber : CharacterBody3D
{
    public bool caught = false;
    public FishBase fish = null;
    public override void _PhysicsProcess(double delta)
    {

        if (!IsOnFloor())
        {
            var forward_dir = -GlobalTransform.Basis.Z;
            Vector3 vel = new Vector3();
            vel.X = Mathf.Clamp(forward_dir.X * 0.5f, -1, 1);
            vel.Z = Mathf.Clamp(forward_dir.Z * 0.5f, -1, 1);
            Velocity += new Vector3(vel.X, GetGravity().Y * (float)delta * 2, vel.Z);
            GD.Print(vel);
        }

        if (IsOnFloor() && Velocity.Y <= 0)
        {
            Velocity = Vector3.Zero;
        }
        MoveAndSlide();
    }
}
