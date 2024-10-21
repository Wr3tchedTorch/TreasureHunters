using Godot;

namespace Game.StateMachine.State;

[Icon("res://assets/godot icons/icon_event.png")]
public partial class FollowState : BaseState
{

	[Signal] public delegate void CloseToTargetEventHandler();

	public override void Enter()
	{
		throw new System.NotImplementedException();
	}

	public override void Exit()
	{
		throw new System.NotImplementedException();
	}

	public override void PhysicsUpdate(double delta)
	{
		throw new System.NotImplementedException();
	}

	public override void Update(double delta)
	{
		throw new System.NotImplementedException();
	}
}
