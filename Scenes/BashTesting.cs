using Godot;
using System;

public partial class BashTesting : CanvasLayer
{



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void RunTestBash()
	{

		Godot.Collections.Array output = new Godot.Collections.Array();
        OS.Execute("C:\\Program Files\\Git\\bin\\bash.exe", new string[] {"-c", "res://Resources/BashScripts/TestBash.sh"}, output);
		GD.Print(output);

    }
}
