using Godot;
using System;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class WanderState : BaseState
{

	[Signal] public delegate void EnemyInSightEventHandler();

	public override void Enter()
	{
		throw new NotImplementedException();
	}

	public override void Exit()
	{
		throw new NotImplementedException();
	}

	public override void PhysicsUpdate(double delta)
	{
		throw new NotImplementedException();
	}

	public override void Update(double delta)
	{
		throw new NotImplementedException();
	}

}
