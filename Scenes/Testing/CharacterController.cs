using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
    [Export] private float _speed = 100;
    [Export] private Vector2 _targetPos;
    public Vector2 TargetPos { get { return _targetPos; } set { _targetPos = value; _navAgent.TargetPosition = value; } }

    private NavigationAgent2D _navAgent;

    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent2D>("CharacterNavAgent");

        _navAgent.PathDesiredDistance = 4.0f;
        _navAgent.TargetDesiredDistance = 4.0f;

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
		
        if(_navAgent.IsNavigationFinished())
        {
            return;
        }

        Vector2 nexPos = _navAgent.GetNextPathPosition();
        Velocity = GlobalTransform.Origin.DirectionTo(nexPos) * _speed;
        MoveAndSlide();

	}
}
