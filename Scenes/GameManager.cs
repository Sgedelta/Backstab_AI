using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance;
    public Dictionary<string, Callable> CommandDict = new Dictionary<string, Callable>()
    {
        {"travel", Callable.From<Array<Variant>>((args) => { ((Survivor)args[0]).Travel((Vector2)args[2]); }) }
    };
    public Dictionary<string, Variant> ArgumentDict = new Dictionary<string, Variant>()
    {
		//TEMP!!
		{"Sur1", new Variant() },
        {"Room1", new Vector2(1, 0) },
        {"Room2" , new Vector2(10, 5)},
        {"Room3" , new Vector2(-50, 100)}
    };

    public GameParent ActiveGameParent;


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

    public void SetupSurvivorData(Array<Survivor> Characters)
    {
        //add all survivors to ArgumentDict here
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    public void ProcessSabreCommand(Array<string> commArray)
    {
        //Note: DOES NOT RESPECT MUTEX! COPY DATA BEFORE PASSING HERE!!
        string commandKey = commArray[0];
        commArray.Remove(commandKey);

        //build args
        Array<Variant> argsArray = new();

        for (int i = 0; i < commArray.Count; i++)
        {
            argsArray.Add(ArgumentDict[commArray[i]]);
        }

        //call function
        CommandDict[commandKey].Call(argsArray);
    }
}
