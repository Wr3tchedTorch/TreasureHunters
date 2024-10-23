using Game.Component;
using Godot;
using System;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class WanderState : BaseState
{

	private const float TURNING_DELAY = .2f;

	[Export] private MovementComponent _movementComponent;
	[Export] private Area2D _groundCollisionDetector;

	[Signal] public delegate void EnemyInSightEventHandler();

	private readonly Random _RNG = new();
	private float _direction;
	private bool _canFlip = true;

	public async override void Enter()
	{

		_groundCollisionDetector.BodyExited += OnGroundBodyExited;

		_direction = -1;
		_movementComponent.SetSpeed(_movementComponent.MovementSpeed / 2.5f);

		_canFlip = false;
		await ToSignal(GetTree().CreateTimer(TURNING_DELAY), "timeout");
		_canFlip = true;
	}

	public override void Exit()
	{
		throw new NotImplementedException();
	}

	public override void PhysicsUpdate(double delta)
	{
		if (!_movementComponent.ParentIsOnFloor())
			return;

		_movementComponent.Walk(_direction);
	}

	public override void Update(double delta)
	{
		// throw new NotImplementedException();
	}

	private int GetRandomDirection()
	{
		int randomDirection = _RNG.Next(-1, 1);
		if (randomDirection == 0)
			return GetRandomDirection();

		return randomDirection;
	}

	private async void OnGroundBodyExited(Node2D body)
	{

		GD.Print(_canFlip);

		if (!_movementComponent.ParentIsOnFloor() || !_canFlip)
			return;

		_direction *= -1;

		_canFlip = false;
		GD.Print("Waiting for timer");
		await ToSignal(GetTree().CreateTimer(TURNING_DELAY), "timeout");
		GD.Print("Timer Timeout");
		_canFlip = true;
	}
}
