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

	[Export] private string _sabreFilePath;


	private Godot.Collections.Array<Godot.Collections.Array<string>> _sabreParsedOutput;
	public Godot.Collections.Array<Godot.Collections.Array<string>> SabreParsedOutput { get { return _sabreParsedOutput; } }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sabreParsedOutput = new Godot.Collections.Array<Godot.Collections.Array<string>>();

        BashThread = new GodotThread();
        BashSema = new Semaphore();
        BashMutex = new Mutex();

        BashThread.Start(Callable.From(PollSabre));

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


	public void RunSabre()
	{
		BashSema.Post();
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
			BashMutex.Lock();
			string sabrePath = _sabreFilePath;
			BashMutex.Unlock();

            Godot.Collections.Array output = new Godot.Collections.Array();

			GD.Print($"[SR] Running Sabre file at {sabrePath}");
            OS.Execute("C:\\Program Files\\Git\\bin\\bash.exe", new string[] { ProjectSettings.GlobalizePath("res://Resources/BashScripts/BashSabre.sh"), $"{sabrePath}" }, output, true);
			GD.Print($"[SR] Sabre Finished Processing, output was:\n   {output}");

			BashMutex.Lock();
			//clear out old output
			_sabreParsedOutput.Clear();

			//split output into usable chunks
			string data = ((string)output[0]);
			data = data.Replace(",", "");
			data = data.Substring(0, data.Length - 2); //trim \r\n
			string[] dataArr = data.Split(')');
			int i = 0; 
			foreach (string s in dataArr)
			{
				//if this is empty, skip it (can happen at end of output)
				if(s.Length <= 0)
				{
					continue;
				}
				
				//add a new subarray
				_sabreParsedOutput.Add(new Godot.Collections.Array<string>());

				//split into componenet parts 
				GD.Print("[SR} splitting:" + s);
				string[] commandArr = s.Trim().Split(new char[] { ' ', '(' });
				foreach(string sub in commandArr)
				{
					GD.Print("[SR]   >" + sub);
					_sabreParsedOutput[i].Add(sub);
				}
				i += 1;
			}


			BashMutex.Unlock();


            //once processing is done, let the main thread know that we are done with a deferred signal call (deferred to resync with main thread. I think.)
            CallDeferred("emit_signal", "SabreCompleted");
			
		}
	}
}
