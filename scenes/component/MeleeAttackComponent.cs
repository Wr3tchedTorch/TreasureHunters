using Godot;
using Game.Resources.Attack;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class MeleeAttackComponent : Node
{

	public AttackResource CurrentAttackInCombo => _attacks[_currentAttackIndex];

	private const float ATTACK_DURATION = .6f;

	[Export] private AttackResource[] _attacks;
	[Export] private FlippingComponent _flippingComponent;

	private Node2D _parent;
	private int _currentAttackIndex = 0;
	private bool _isAttacking = false;

	public override void _Ready()
	{

		_parent = GetOwner<Node2D>();
	}

	public async void Attack()
	{

		if (_isAttacking)
			return;

		_isAttacking = true;

		var _currentAttack = CurrentAttackInCombo.AttackScene.Instantiate<Area2D>();
		_currentAttack.BodyEntered += OnCurrentAttackBodyEntered;

		var collisionShape = _currentAttack.GetNode<CollisionShape2D>("CollisionShape2D");
		Vector2 position = collisionShape.Position;
		position.X *= _flippingComponent.IsParentFacingLeft ? -1 : 1;
		collisionShape.Position = position;

		_parent.AddChild(_currentAttack);

		await ToSignal(GetTree().CreateTimer(ATTACK_DURATION), "timeout");

		_isAttacking = false;
		_currentAttack.QueueFree();
		IncreaseAttackIndex();
	}

	private void IncreaseAttackIndex()
	{

		_currentAttackIndex++;
		if (_currentAttackIndex >= _attacks.Length)
			_currentAttackIndex = 0;
	}

	private void OnCurrentAttackBodyEntered(Node2D body)
	{
		GD.Print("HITTING AN ENEMY!");
	}
}
