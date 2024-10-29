using Godot;
using Game.Component;

namespace Game.Player;

[Icon("res://assets/godot icons/icon_character.png")]
public partial class Pirate : CharacterBody2D
{

    private readonly StringName ACTION_LEFT = "left";
    private readonly StringName ACTION_RIGHT = "right";
    private readonly StringName ACTION_JUMP = "jump";
    private readonly StringName ACTION_ATTACK = "attack";

    private MovementComponent _movementComponent;
    private MeleeAttackComponent _meleeAttackComponent;

    public override void _Ready()
    {

        _meleeAttackComponent = GetNode<MeleeAttackComponent>(nameof(MeleeAttackComponent));
        _movementComponent = GetNode<MovementComponent>(nameof(MovementComponent));
    }

    public override void _PhysicsProcess(double delta)
    {

        if (Input.IsActionJustPressed(ACTION_ATTACK))
            _meleeAttackComponent.Attack();

        if (Input.IsActionPressed(ACTION_JUMP))
            _movementComponent.Jump();

        _movementComponent.Walk(GetHorizontalMovementDirection());
    }

    public float GetHorizontalMovementDirection()
        => Input.GetAxis(ACTION_LEFT, ACTION_RIGHT);
}
