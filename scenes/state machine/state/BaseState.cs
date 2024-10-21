using Godot;

namespace Game.StateMachine.State;

[GlobalClass]
public abstract partial class BaseState : Node
{

	public abstract void Enter();
	public abstract void Exit();
	public abstract void Update(double delta);
	public abstract void PhysicsUpdate(double delta);
}
