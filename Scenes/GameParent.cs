using Godot;
using Godot.Collections;
using System;

public partial class GameParent : Node2D
{
	[Export]
	public Array<RoomParent> Rooms = new Array<RoomParent>();

	[Export] public Array<Survivor> Characters = new Array<Survivor>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.ActiveGameParent = this;
		GameManager.Instance.SetupSurvivorData(Characters);
		SetActiveCameraToKey(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetActiveCameraToKey(int key)
	{
		Rooms[key].RoomCam.Enabled = true;
        Rooms[key].RoomCam.MakeCurrent();

		//Control UI = GetNode<Control>("CameraButtonInterpretor");
		//UI.GlobalPosition = Rooms[key].RoomCam.GlobalPosition;
		//UI.Scale = Rooms[key].RoomCam.Zoom;
    }

}
