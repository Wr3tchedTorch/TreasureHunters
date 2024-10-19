using System.Security;
using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class VelocityComponent : Node2D
{

	[Export] private float _movementSpeed = 200;

	private CharacterBody2D _parent;
	private float _walkForce;
	private float _jumpForce;

	public override void _Ready()
	{

		_parent = GetParent<CharacterBody2D>();
	}

	public override void _Process(double delta)
	{

		_parent.Velocity = new Vector2(_walkForce, 0);
		_parent.MoveAndSlide();

		_walkForce = 0;
		_jumpForce = 0;
	}

	public void Jump()
	{

	}

	public void Walk(float direction)
	{
		_walkForce = direction * _movementSpeed;
	}
}
