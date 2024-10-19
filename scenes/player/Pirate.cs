using Godot;
using Game.Component;

namespace Game.Player;

[Icon("res://assets/godot icons/icon_character.png")]
public partial class Pirate : CharacterBody2D
{

    private VelocityComponent _velocityComponent;

    public override void _Ready()
    {

        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
    }

    public override void _Process(double delta)
    {

        _velocityComponent.Walk(GetHorizontalMovementDirection());
    }

    public float GetHorizontalMovementDirection()
    {
        var left = Input.IsActionPressed("left") ? 1 : 0;
        var right = Input.IsActionPressed("right") ? 1 : 0;
        return right - left;
    }
}
