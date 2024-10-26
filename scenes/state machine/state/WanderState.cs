using Game.Component;
using Godot;
using System;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class WanderState : BaseState
{

	private readonly float TURNING_DELAY = .2f;

	[Export] private MovementComponent _movementComponent;
	[Export] private Area2D _collisionDetector;
	[Export] private RayCast2D _enemiesSight;
	[Export] private float _minIdleDelayBeforeTurning;
	[Export] private float _maxIdleDelayBeforeTurning;

	[Signal] public delegate void EnemyInSightEventHandler();

	private readonly Random _RNG = new();
	private float _direction;
	private float _previousDirection;
	private bool _canTurn = true;

	public async override void Enter()
	{

		_collisionDetector.BodyExited += OnCollisionDetectorBodyExited;

		_direction = GetRandomDirection();
		_movementComponent.SetSpeed(_movementComponent.MovementSpeed / 2.5f);

		_canTurn = false;
		await ToSignal(GetTree().CreateTimer(TURNING_DELAY), "timeout");
		_canTurn = true;
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
		if (_enemiesSight.IsColliding())
			Turn();
	}

	private async void Turn()
	{
		if (!_movementComponent.ParentIsOnFloor() || !_canTurn)
			return;

		_previousDirection = _direction;
		_direction = 0;

		float awaitTimer = Math.Min(((float)_RNG.NextDouble() * _maxIdleDelayBeforeTurning) + _minIdleDelayBeforeTurning, _maxIdleDelayBeforeTurning);
		await ToSignal(GetTree().CreateTimer(awaitTimer), "timeout");
		_direction = -_previousDirection;

		_canTurn = false;
		await ToSignal(GetTree().CreateTimer(TURNING_DELAY), "timeout");
		_canTurn = true;
	}

	private int GetRandomDirection()
	{
		int randomDirection = _RNG.Next(-1, 1);
		if (randomDirection == 0)
			return GetRandomDirection();

		return randomDirection;
	}

	private void OnCollisionDetectorBodyExited(Node2D body)
	{
		if (body is not CharacterBody2D)
			Turn();
	}
}
