using Godot;
using System;

public partial class Survivor : Node2D
{
	[Export] public string SurvName;
	[Export] public bool IsImposter;
	[Export] public RoomParent CurrentRoom;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Travel(Vector2 loc)
	{
		GD.Print($"{SurvName} goes to {loc}");
	}
}
