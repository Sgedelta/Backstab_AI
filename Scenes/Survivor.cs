using Godot;
using System;

//public enum CurrentAction
//{
//    travel,
//    kill
//}

public partial class Survivor : Node2D
{
    [Export] public string SurvName;
    [Export] public bool IsImposter;
    [Export] public RoomParent CurrentRoom;

    private RoomParent currentRoom;
    private CurrentAction currentAction;
    private Variant target;

    private RandomNumberGenerator rng;

    public bool temp = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rng = new RandomNumberGenerator();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (temp)
        {
            Travel(GameParent.Instance.Rooms[0]);
            temp = false;
        }
        switch (currentAction)
        {
            case CurrentAction.travel:
                if (CurrentRoom != (RoomParent)target) Travel((RoomParent)target);
                else
                {
                    Wander();
                }
                break;
            case CurrentAction.kill:
                break;
            default:
                break;
        }
    }

    public void Travel(RoomParent room)
    {
        //nav agent pathfind to room.RoomLocation
        currentRoom = room;
        GD.Print($"{SurvName} goes to {room}");
        this.Position = room.Position;
    }

    public void Wander()
    {
        Vector2 moveToPos = new Vector2(rng.RandfRange(currentRoom.MinRoomBounds[0], currentRoom.MaxRoomBounds[0]), rng.RandfRange(currentRoom.MinRoomBounds[1], currentRoom.MaxRoomBounds[1]));
        //temp
        Position = moveToPos;
        //nav agent pathfind to moveToPos
    }

    public void SetGoal(CurrentAction action, Variant target)
    {
        currentAction = action;
        this.target = target;
    }
}
