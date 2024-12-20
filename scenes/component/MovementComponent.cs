using Game.Player;
using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class MovementComponent : Node
{

	public float Speed { get => _speed ?? _defaultSpeed; set => _speed = value; }
	public bool IsOnFloor => _parent.IsOnFloor();

	private readonly string ANIMATION_IDLE = "idle";
	private readonly string ANIMATION_WALK = "walk";

	[ExportGroup("Dependencies")]
	[Export] private AnimationComponent _animationComponent;
	[Export] private VelocityComponent _velocityComponent;
	[Export] private FlippingComponent _flippingComponent;

	[ExportGroup("Movement Physics")]
	[Export] private float _defaultSpeed = 100;

	[ExportGroup("Jump Physics")]
	[Export] private float _jumpHeight = 100.0f;
	[Export] private float _jumpTimeToPeak = 0.5f;
	[Export] private float _jumpTimeToDescent = 0.4f;
	[Export] private float _coyoteTime = 0.25f;

	private CharacterBody2D _parent;
	private float? _speed = null;
	private float _jumpVelocity;
	private float _jumpGravity;
	private float _fallGravity;
	private bool _canJump;
	private bool _wasOnFloor;

	public override void _Ready()
	{
		Speed = _defaultSpeed;
		_parent = GetOwner<CharacterBody2D>();

		_jumpVelocity = 2.0f * _jumpHeight / _jumpTimeToPeak * -1;
		_jumpGravity = -2.0f * _jumpHeight / (_jumpTimeToPeak * _jumpTimeToPeak) * -1;
		_fallGravity = -2.0f * _jumpHeight / (_jumpTimeToDescent * _jumpTimeToDescent) * -1;
	}

	public override void _PhysicsProcess(double delta)
	{

		if (!IsOnFloor && _canJump && _wasOnFloor)
			GetTree().CreateTimer(_coyoteTime).Timeout += OnCoyoteTimerTimeout;

		if (IsOnFloor)
		{

			_canJump = true;
			if (!_wasOnFloor)
				_velocityComponent.OverrideVelocityY(0);
		}

		if (!IsOnFloor)
			_velocityComponent.OverrideVelocityY(_velocityComponent.Velocity.Y + GetGravity() * (float)delta);

		_wasOnFloor = IsOnFloor;
	}

	public void Walk(float direction)
	{

		if (direction == 0)
		{
			_animationComponent.SetAnimation(ANIMATION_IDLE);
			_velocityComponent.DecelerateX();
			return;
		}

		_animationComponent.SetAnimation(ANIMATION_WALK);
		_flippingComponent.FlipH(direction == -1);

		_velocityComponent.AccelerateX(direction, Speed);
	}

	public void Jump()
	{
		if (!_canJump)
			return;

		_canJump = false;
		_velocityComponent.OverrideVelocityY(_jumpVelocity);
	}

	private float GetGravity()
	{

		if (_parent.IsOnFloor())
			return 0;

		return _velocityComponent.Velocity.Y < 0 ? _jumpGravity : _fallGravity;
	}

	private void OnCoyoteTimerTimeout()
		=> _canJump = false;
}
