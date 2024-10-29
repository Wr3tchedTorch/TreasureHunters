using Game.Component;
using Godot;
using System;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class WanderState : BaseState
{

	private readonly float TURNING_DELAY = 0.25f;
	private readonly float MIN_WANDER_SLOWNESS = 4.5f;
	private readonly float MAX_WANDER_SLOWNESS = 7.5f;

	[Export] private MovementComponent _movementComponent;
	[Export] private Area2D _collisionDetector;
	[Export] private float _minIdleDelayBeforeTurning = 0.25f;
	[Export] private float _maxIdleDelayBeforeTurning = 6.5f;
	[Export(PropertyHint.Range, "-1,1")] private int _initialDirection;

	[Signal] public delegate void EnemyInSightEventHandler();

	private readonly Random _RNG = new();
	private float _direction;
	private float _previousDirection;
	private bool _canTurn = true;

	private float WanderSpeed => _movementComponent.Speed / Mathf.Clamp((float)_RNG.NextDouble() * MAX_WANDER_SLOWNESS, MIN_WANDER_SLOWNESS, MAX_WANDER_SLOWNESS);
	private float TurningDelay => Mathf.Clamp((float)_RNG.NextDouble() * _maxIdleDelayBeforeTurning, _minIdleDelayBeforeTurning, _maxIdleDelayBeforeTurning);

	public override void Enter()
	{

		_collisionDetector.BodyExited += OnCollisionDetectorBodyExited;
		_collisionDetector.BodyEntered += OnCollisionDetectorBodyEntered;

		_direction = _initialDirection != 0 ? _initialDirection : GetRandomDirection();
		_movementComponent.Speed = WanderSpeed;

		StartCanTurnTimer();
	}

	public override void Update(double delta) { }

	public override void Exit() { }

	public override void PhysicsUpdate(double delta)
	{
		if (!_movementComponent.IsOnFloor)
			return;

		_movementComponent.Walk(_direction);
	}

	private async void Turn(bool isIgnoringCanTurn)
	{
		if (!_movementComponent.IsOnFloor || (!isIgnoringCanTurn && !_canTurn))
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
			Turn(true);
	}

	private void OnCollisionDetectorBodyExited(Node2D body)
	{
		if (body is not CharacterBody2D)
			Turn(false);
	}
}
