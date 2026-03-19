	using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;


	public Godot.Collections.Dictionary<string, Callable> CommandDict = new Godot.Collections.Dictionary<string, Callable>()
	{
		{"travel", Callable.From<Godot.Collections.Array<Variant>>((args) => { Instance.Travel((Node)args[0], (Vector2)args[2]); }) }
	};
	public Godot.Collections.Dictionary<string, Variant> ArgumentDict = new Godot.Collections.Dictionary<string, Variant>()
	{
		//TEMP!!
		{"Sur1", new Variant() },
		{"Room1", new Vector2(1, 0) },
		{"Room2" , new Vector2(10, 5)},
		{"Room3" , new Vector2(-50, 100)}
	};


    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
			Callable.From(() => { SetupGameData(); }).CallDeferred();
            this.ProcessMode = ProcessModeEnum.Always;
        }
        else
        {
            GD.PrintErr($"[GM] Two GameManagers Created! Deleting self. (from {Name})");
            QueueFree();
        }

    }


	public void SetupGameData()
	{
		ArgumentDict["Sur1"] = Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void ProcessSabreCommand(Godot.Collections.Array<string> commArray)
	{
		//Note: DOES NOT RESPECT MUTEX! COPY DATA BEFORE PASSING HERE!!
		string commandKey = commArray[0];
		commArray.Remove(commandKey);

		//build args
		Godot.Collections.Array<Variant> argsArray = new();

		for (int i = 0; i < commArray.Count; i++)
		{
			argsArray.Add(ArgumentDict[commArray[i]]);
		}

		//call function
		CommandDict[commandKey].Call(argsArray);
	}



	public void Travel(Node character, Vector2 target)
	{
		//implement travel here
		//TEMP
		GD.Print($"{character.Name} travels to {target}");
	}


}
