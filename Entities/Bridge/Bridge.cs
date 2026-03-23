using Godot;
using System;

public partial class Bridge : MeshInstance3D
{
	private bool isRaisedAlready = false;
	// Raises the bridge when the appear function is called from a signal.
	public void Appear()
	{
		if (!isRaisedAlready)
		{
			Position += new Vector3(0, 5, 0);
			isRaisedAlready = true;
		}
	}
}
