using Godot;

namespace Game.Collision;

public partial class CollisionFlipper : CollisionShape2D
{

	[Export] private Vector2 _leftPosition;
	[Export] private Vector2 _rightPosition;

	public void Flip(bool isFlip)
	{
		Position = isFlip ? _leftPosition : _rightPosition;
	}
}
