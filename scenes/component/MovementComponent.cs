using Godot;
using Game.Collision;
using System.Linq;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class MovementComponent : Node
{

	private readonly string ANIMATION_IDLE = "idle";
	private readonly string ANIMATION_WALK = "walk";

	[ExportGroup("Dependencies")]
	[Export] private AnimationComponent _animationComponent;

	[ExportGroup("Flipping")]
	[Export] private bool _isSpriteLeftOriented;
	[Export] private AnimatedSprite2D[] _animatedSprites;
	[Export] private Sprite2D[] _sprites;
	[Export] private CollisionShape2D[] _collisionShapes;
	[Export] private CollisionPolygon2D[] _collisionPolygons;

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
	private Vector2 _velocity;
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
			_animationComponent.SetAnimation(ANIMATION_IDLE);
			AddFriction();
			return;
		}

		_animationComponent.SetAnimation(ANIMATION_WALK);
		FlipH(direction == -1);
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
		=> MovementSpeed = newSpeed;

	public bool ParentIsOnFloor()
		=> _parent.IsOnFloor();

	private float GetGravity()
	{

		if (_parent.IsOnFloor())
			return 0;

		return _velocity.Y < 0 ? _jumpGravity : _fallGravity;
	}

	private void FlipH(bool flip)
	{

		if (_sprites.Length > 0)
			FlipSprites(flip, _sprites);

		if (_animatedSprites.Length > 0)
			FlipSprites(flip, _animatedSprites);

		if (_collisionShapes.Length > 0)
			FlipCollisionShapes(flip);

		GD.Print(_collisionPolygons.Length);
		if (_collisionPolygons.Length > 0)
			FlipCollisionPolygons(flip);
	}

	private void FlipCollisionPolygons(bool flip)
	{

		foreach (CollisionPolygon2D polygon in _collisionPolygons)
		{
			var scale = polygon.Scale;
			scale.X = flip ? -1 : 1;
			polygon.Scale = scale;
		}
	}

	private void FlipSprites(bool flip, Node2D[] arr)
	{

		if (_isSpriteLeftOriented)
			flip = !flip;

		foreach (var sprite in arr)
		{

			if (sprite is Sprite2D sprite2D)
				sprite2D.FlipH = flip;
			if (sprite is AnimatedSprite2D animSprite2D)
				animSprite2D.FlipH = flip;
		}
	}

	private void FlipCollisionShapes(bool flip)
	{
		foreach (CollisionFlipper collisionShape in _collisionShapes.Cast<CollisionFlipper>())
		{
			collisionShape.Flip(flip);
		}
	}

	private void HorizontalAccel(float direction)
		=> _velocity.X = Mathf.MoveToward(_velocity.X, direction * MovementSpeed, _acceleration);

	private void AddFriction()
		=> _velocity.X = Mathf.MoveToward(_velocity.X, 0, _friction);

	private void OnCoyoteTimerTimeout()
		=> _canJump = false;
}
