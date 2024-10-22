using Game.Component;
using Godot;
using System;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class WanderState : BaseState
{

	[Export] private MovementComponent _movementComponent;
	[Export] private RayCast2D _frontRayCast;
	[Export] private RayCast2D _rightRayCast;
	[Export] private RayCast2D _leftRayCast;
	[Export] private double _movementMaxDelay = 1.5;
	[Export] private double _movementMinDelay = 0.3;

	[Signal] public delegate void EnemyInSightEventHandler();

	private Timer _movementTimer;
	private Timer _turningTimer;
	private readonly Random _RNG = new();
	private float _direction;
	private bool _canTurn = true;

	public override void Enter()
	{

		_turningTimer = GetNode<Timer>("TurningTimer");
		_movementTimer = GetNode<Timer>("MovementTimer");

		_turningTimer.WaitTime = 0.5;
		_turningTimer.Timeout += OnTurningTimerTimeout;

		_movementTimer.WaitTime = GetRandomWaitTime();
		_movementTimer.Timeout += OnMovementTimerTimeout;
		_movementTimer.Start();

		_direction = GetRandomDirection();
		_movementComponent.SetSpeed(_movementComponent.MovementSpeed / 2.5f);
	}

	public override void Exit()
	{
		throw new NotImplementedException();
	}

	public override void PhysicsUpdate(double delta)
	{

		bool isAboutToFall = !_rightRayCast.IsColliding() || !_leftRayCast.IsColliding();
		if (_canTurn && (isAboutToFall || _frontRayCast.IsColliding()))
		{
			GD.Print("TURNING!!!");
			_direction *= -1;
			_canTurn = false;
			_turningTimer.Start();
		}

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

	private double GetRandomWaitTime()
		=> Math.Min((_RNG.NextDouble() * _movementMaxDelay) + _movementMinDelay, _movementMaxDelay);

	private void OnMovementTimerTimeout()
	{
	}

	private void OnTurningTimerTimeout()
	{
		_canTurn = true;
	}
}
