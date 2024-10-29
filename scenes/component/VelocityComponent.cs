using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class VelocityComponent : Node
{

	public Vector2 Velocity { get; set; }

	[Export] private float _accel = 40;
	[Export] private float _friction = 20;

	private CharacterBody2D _parent;

	public override void _Ready()
	{

		_parent = GetOwner<CharacterBody2D>();
	}

	public override void _PhysicsProcess(double delta)
	{

		_parent.Velocity = Velocity;
		_parent.MoveAndSlide();
	}

	public void OverrideVelocityX(float xTo)
	{
		Velocity = new Vector2(xTo, Velocity.Y);
	}

	public void OverrideVelocityY(float yTo)
	{
		Velocity = new Vector2(Velocity.X, yTo);
	}

	public void AccelerateX(float toDir, float speed)
	{

		var _velocity = Velocity;
		_velocity.X = Mathf.MoveToward(_velocity.X, toDir * speed, _accel);
		Velocity = _velocity;
	}

	public void Decelerate()
	{
		Velocity = Velocity.MoveToward(Vector2.Zero, _friction);
	}

	public void DecelerateX()
	{
		Velocity = Velocity.MoveToward(new Vector2(0, Velocity.Y), _friction);
	}
}
