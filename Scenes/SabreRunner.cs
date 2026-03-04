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
	bool exitThread = false;

	[Signal] public delegate void SabreCompletedEventHandler();


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

    public override void _ExitTree()
    {
		base._ExitTree();
		//we have to safely close the thread before we exit the tree
		ExitThread();
    }

	/// <summary>
	/// Frees the running thread - MIGHT cause blocking, but is needed to exit safely
	/// </summary>
	public void ExitThread()
	{
		BashMutex.Lock();
		exitThread = true;
		BashMutex.Unlock();

		BashSema.Post();

		BashThread.WaitToFinish();	
	}


	public void PollSabre()
	{
		//keep looping until we are told to break with exitThread
		while (true)
		{
			BashSema.Wait(); //hold until we want to run something

			//check if we've gotten the end signal
			BashMutex.Lock();
			bool leave = exitThread;
			BashMutex.Unlock();

			if(leave)
			{
				break;
			}

			//now do our processing




			//once processing is done, let the main thread know that we are done with a deferred signal call (deferred to resync with main thread. I think.)
			CallDeferred("emit_signal", "SabreCompleted");
			
		}
	}
}
