using Godot;

namespace Game.Component;

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

	public void Accelerate(Vector2 toDir, float speed)
	{

		Velocity = Velocity.MoveToward(toDir.Normalized() * speed, _accel);
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
