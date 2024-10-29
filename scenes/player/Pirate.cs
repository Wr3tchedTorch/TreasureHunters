using Godot;
using Game.Component;

namespace Game.Player;

[Icon("res://assets/godot icons/icon_character.png")]
public partial class Pirate : CharacterBody2D
{

    private MovementComponent _movementComponent;    

    public override void _Ready()
    {

        _movementComponent = GetNode<MovementComponent>(nameof(MovementComponent));
    }

    public override void _PhysicsProcess(double delta)
    {        

        if (Input.IsActionPressed("jump"))
            _movementComponent.Jump();

        _movementComponent.Walk(GetHorizontalMovementDirection());
    }

    public float GetHorizontalMovementDirection()
        => Input.GetAxis("left", "right");
}
