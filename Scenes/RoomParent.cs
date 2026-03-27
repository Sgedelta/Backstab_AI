using Godot;
using System;

public partial class RoomParent : Node2D
{
	public Vector2 RoomLocation;
	public Camera2D RoomCam;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RoomLocation = this.Position;
		RoomCam = GetNode<Camera2D>("RoomCam");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
