using Godot;
using System;

/// <summary>
/// Our main interface with Sabre, via bash scripts
/// </summary>
public partial class SabreRunner : Node
{
	GodotThread BashThread;
	Semaphore BashSema;
	Mutex BashMutex;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        BashThread = new GodotThread();
        BashSema = new Semaphore();
        BashMutex = new Mutex();

        //BashThread.Start(Callable.From());

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
