using Godot;

namespace Game.Collision;

public partial class RayCastFlipper : RayCast2D
{

	[Export] private Vector2 _leftPosition;
	[Export] private Vector2 _rightPosition;

	public void Flip(bool isFlip)
	{
		TargetPosition = isFlip ? _leftPosition : _rightPosition;
	}
}
