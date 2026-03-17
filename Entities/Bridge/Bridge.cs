using Godot;
using System;

public partial class Bridge : MeshInstance3D
{
	public void Appear()
	{
		Position += new Vector3(0, 5, 0);
	}
}
