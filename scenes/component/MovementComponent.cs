using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class MovementComponent : Node
{

	[ExportGroup("Horizontal Movement")]
	[Export] public float MovementSpeed { get; private set; } = 180.0f;
	[Export] private float _acceleration = 40.0f;
	[Export] private float _friction = 20.0f;

	[ExportGroup("Jump Physics")]
	[Export] private float _jumpHeight = 100.0f;
	[Export] private float _jumpTimeToPeak = 0.5f;
	[Export] private float _jumpTimeToDescent = 0.4f;
	[Export] private float _coyoteTime = 0.25f;

	private CharacterBody2D _parent;
	private Vector2 _velocity = Vector2.Zero;
	private float _jumpVelocity;
	private float _jumpGravity;
	private float _fallGravity;
	private bool _canJump;

	public override void _Ready()
	{

		_parent = GetParent<CharacterBody2D>();

		_jumpVelocity = 2.0f * _jumpHeight / _jumpTimeToPeak * -1;
		_jumpGravity = -2.0f * _jumpHeight / (_jumpTimeToPeak * _jumpTimeToPeak) * -1;
		_fallGravity = -2.0f * _jumpHeight / (_jumpTimeToDescent * _jumpTimeToDescent) * -1;
	}

	public override void _PhysicsProcess(double delta)
	{

		if (!_parent.IsOnFloor() && _canJump)
			GetTree().CreateTimer(_coyoteTime).Timeout += OnCoyoteTimerTimeout;

		_parent.Velocity = _velocity;
		_parent.MoveAndSlide();

		if (_parent.IsOnFloor())
		{
			_canJump = true;
			_velocity.Y = 0;
		}

		_velocity.Y += GetGravity() * (float)delta;
	}

	public void Walk(float direction)
	{

		if (direction == 0)
		{
			AddFriction();
			return;
		}
		HorizontalAccel(direction);
	}

	public void Jump()
	{

		if (!_canJump)
			return;

		_canJump = false;
		_velocity.Y = _jumpVelocity;
	}

	public void SetSpeed(float newSpeed)
	{
		MovementSpeed = newSpeed;
	}

	private float GetGravity()
	{

		if (_parent.IsOnFloor())
			return 0;

		return _velocity.Y < 0 ? _jumpGravity : _fallGravity;
	}

	private void HorizontalAccel(float direction)
		=> _velocity.X = Mathf.MoveToward(_velocity.X, direction * MovementSpeed, _acceleration);

	private void AddFriction()
		=> _velocity.X = Mathf.MoveToward(_velocity.X, 0, _friction);

	private void OnCoyoteTimerTimeout()
		=> _canJump = false;
}
