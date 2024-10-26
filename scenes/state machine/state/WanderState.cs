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
	[Export] private float _minIdleDelayBeforeTurning = 0.25f;
	[Export] private float _maxIdleDelayBeforeTurning = 3.1f;
	[Export(PropertyHint.Range, "-1,1")] private int _initialDirection;

	[Signal] public delegate void EnemyInSightEventHandler();

	private readonly Random _RNG = new();
	private float _direction;
	private float _previousDirection;
	private bool _canTurn = true;

	private float WanderSpeed => _movementComponent.MovementSpeed / Mathf.Clamp((float)_RNG.NextDouble() * 5f, 3.5f, 5.2f);
	private float TurningDelay => Mathf.Clamp((float)_RNG.NextDouble() * _maxIdleDelayBeforeTurning, _minIdleDelayBeforeTurning, _maxIdleDelayBeforeTurning);

	public override void Enter()
	{

		_collisionDetector.BodyExited += OnCollisionDetectorBodyExited;
		_collisionDetector.BodyEntered += OnCollisionDetectorBodyEntered;

		_direction = _initialDirection != 0 ? _initialDirection : GetRandomDirection();
		_movementComponent.SetSpeed(WanderSpeed);

		StartCanTurnTimer();
	}

	public override void Update(double delta) { }

	public override void Exit() { }

	public override void PhysicsUpdate(double delta)
	{
		if (!_movementComponent.ParentIsOnFloor())
			return;

		_movementComponent.Walk(_direction);
	}

	private async void Turn()
	{
		if (!_movementComponent.ParentIsOnFloor() || !_canTurn)
			return;

		_previousDirection = _direction;
		_direction = 0;

		await ToSignal(GetTree().CreateTimer(TurningDelay), "timeout");
		_direction = -_previousDirection;

		StartCanTurnTimer();
	}

	private int GetRandomDirection()
	{
		int randomDirection = _RNG.Next(-1, 1);
		if (randomDirection == 0)
			return GetRandomDirection();

		return randomDirection;
	}

	private async void StartCanTurnTimer()
	{
		_canTurn = false;
		await ToSignal(GetTree().CreateTimer(TURNING_DELAY), "timeout");
		_canTurn = true;
	}

	private void OnCollisionDetectorBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D)
			Turn();
	}

	private void OnCollisionDetectorBodyExited(Node2D body)
	{
		if (body is not CharacterBody2D)
			Turn();
	}
}
