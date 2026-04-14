using Godot;
using System;

public enum CurrentAction
{
    travel,
    wander,
    kill
}
public partial class CharacterController : CharacterBody2D
{
    [Export] private float _speed = 300;
    [Export] private Vector2 _targetPos;
    public Vector2 TargetPos { get { return _targetPos; } set { _targetPos = value; _navAgent.TargetPosition = value; } }

    private NavigationAgent2D _navAgent;

    [Export] public bool IsKiller = false;
    [Export] public RoomParent currentRoom;
    private RoomParent travelToRoom;
    private CurrentAction currentAction;
    private Variant target;
    private RandomNumberGenerator rng;

    bool once = true;
    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent2D>("CharacterNavAgent");

        _navAgent.PathDesiredDistance = 4.0f;
        _navAgent.TargetDesiredDistance = 4.0f;

        rng = new RandomNumberGenerator();

        currentAction = CurrentAction.wander;
        //don't await on ready...
        Callable.From(Setup).CallDeferred();
    }

    private async void Setup()
    {
        //wait for physics frame (lets nav server uh... nav server.)
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        //now set target
        _navAgent.TargetPosition = _targetPos;
    }


    public override void _PhysicsProcess(double delta)
    {
        if (once)
        {
            int toMove = rng.RandiRange(0, GameParent.Instance.Rooms.Count - 1);
            Travel(GameParent.Instance.Rooms[toMove]);
            once = false;
        }

        if (IsKiller)
        {
            if(GameParent.Instance.GetSurInRoom(currentRoom).Count == 2)
            {
                Kill();
            }
        }

        switch (currentAction)
        {
            case CurrentAction.wander:
                if (_navAgent.IsNavigationFinished())
                {
                    Wander();
                    return;
                }
                Vector2 nexPosWander = _navAgent.GetNextPathPosition();
                Velocity = GlobalTransform.Origin.DirectionTo(nexPosWander) * (_speed/100);
                MoveAndSlide();
                break;
            case CurrentAction.travel:
                if (_navAgent.IsNavigationFinished())
                {
                    currentRoom = travelToRoom;
                    Wander();
                    return;
                }
                Vector2 nexPosTravel = _navAgent.GetNextPathPosition();
                Velocity = GlobalTransform.Origin.DirectionTo(nexPosTravel) * _speed;
                MoveAndSlide();               
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
        //currentRoom = room;
        //GD.Print($"{SurvName} goes to {room}");
        //this.Position = room.Position;
        GD.Print("travel called");
        TargetPos = room.Position;
        currentRoom = null;
        currentAction = CurrentAction.travel;
        travelToRoom = room;
    }
    public void Wander()
    {
        GD.Print("wander called");
        Vector2 moveToPos = new Vector2(rng.RandfRange(currentRoom.MinRoomBounds[0], currentRoom.MaxRoomBounds[0]), rng.RandfRange(currentRoom.MinRoomBounds[1], currentRoom.MaxRoomBounds[1]));
        //temp
        //Position = moveToPos;
        TargetPos = moveToPos;
        //nav agent pathfind to moveToPos
    }
    public void Kill()
    {
        GD.Print("kill called");
    }
    public void SetGoal(CurrentAction action, Variant target)
    {
        currentAction = action;
        this.target = target;
    }
}
