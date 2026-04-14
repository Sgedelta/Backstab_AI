using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance;
    public Dictionary<string, Callable> CommandDict = new Dictionary<string, Callable>()
    {
        {"travel", Callable.From<Array<Variant>>((args) => { ((Survivor)args[0]).Travel((RoomParent)args[2]); }) },
        {"kill", Callable.From<Array<Variant>>((args) => {((Survivor)args[0]).SetGoal(CurrentAction.travel, args[2]); }) }
    };
    public Dictionary<string, Variant> ArgumentDict = new Dictionary<string, Variant>()
    {
		//TEMP!!
		{"Sur1", new Variant() },
        {"Room1", new Vector2(1, 0) },
        {"Room2" , new Vector2(10, 5)},
        {"Room3" , new Vector2(-50, 100)}
    };
    //Do we just update this a bunch and then name the objects the same as in Saber?
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
        if (GameParent.Instance.Characters == null || GameParent.Instance.Characters.Count == 0)
        {
            GD.PushError("FUCK (we ain't got no survivors champ)");
            //return;
        }
        if (GameParent.Instance.Rooms == null || GameParent.Instance.Rooms.Count == 0)
        {
            GD.PushError("No rooms to compile");
            //return;
        }
        ArgumentDict["Sur1"] = GameParent.Instance.Characters[0];
        
    }

    public void SetupSurvivorData(Array<CharacterController> Characters)
    {
        //add all survivors and rooms to ArgumentDict here
        //for (int i = 1; i < GameParent.Instance.Characters.Count + 1; i++)
        //{
        //    ArgumentDict.Add($"Sur{i}", GameParent.Instance.Characters[i - 1]);
        //}
        //for (int i = 1; i < GameParent.Instance.Rooms.Count + 1; i++)
        //{
        //    ArgumentDict.Add($"Room{i}", GameParent.Instance.Rooms[i - 1]);
        //}
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
