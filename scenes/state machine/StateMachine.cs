using System;
using System.Collections.Generic;
using Game.StateMachine.State;
using Godot;

namespace Game.StateMachine;

[Icon("res://assets/godot icons/icon_follow.png")]
public partial class StateMachine : Node
{

	public BaseState CurrentState { get; private set; }

	[Export(PropertyHint.NodeType, $"{nameof(BaseState)}")] private BaseState InitialState { get; set; }

	private readonly Dictionary<string, BaseState> _states = new();

	public override void _Ready()
	{

		foreach (Node child in GetChildren())
		{
			if (child is BaseState state)
				_states.Add(state.Name, state);
		}
		CurrentState = InitialState;
		CurrentState.Enter();
	}

	public override void _Process(double delta)
	{
		CurrentState?.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.PhysicsUpdate(delta);
	}

	public void TransitionTo(string stateName)
	{
		if (!_states.ContainsKey(stateName))
			throw new ArgumentException($"The state `{stateName}` is not a valid state in this {nameof(StateMachine)}.");

		if (stateName == CurrentState.Name)
		{
			GD.Print($"{this}: nameof{CurrentState} is already the state {stateName}");
			return;
		}

		CurrentState.Exit();
		CurrentState = _states[stateName];
		CurrentState.Enter();
	}
}
