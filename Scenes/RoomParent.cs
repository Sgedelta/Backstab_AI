using Godot;
using System;

public partial class RoomParent : Node2D
{
	public Vector2 RoomLocation;
	public Camera2D RoomCam;
	public Vector2 MinRoomBounds;
	public Vector2 MaxRoomBounds;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ColorRect room = GetNode<ColorRect>("ColorRect");
        RoomLocation = this.Position;
		MinRoomBounds = GlobalPosition - (room.Size / 2);// new Vector2(GlobalPosition.X - (room.Size.X / 2), GlobalPosition.Y - (room.Size / 2));
		MaxRoomBounds = GlobalPosition + (room.Size / 2);
		RoomCam = GetNode<Camera2D>("RoomCam");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
