using Godot;
using System;

public partial class Bridge : MeshInstance3D
{
	// Raises the bridge when the appear function is called from a signal.
	public void Appear()
	{
		Position += new Vector3(0, 5, 0);
	}
}
