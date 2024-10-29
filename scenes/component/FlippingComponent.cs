using System.Linq;
using Game.Collision;
using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class FlippingComponent : Node
{

	[Export] private bool _isSpriteLeftOriented;
	[Export] private AnimatedSprite2D[] _animatedSprites;
	[Export] private Sprite2D[] _sprites;
	[Export] private CollisionShape2D[] _collisionShapes;
	[Export] private CollisionPolygon2D[] _collisionPolygons;

	public void FlipH(bool flip)
	{
		if (_sprites.Length > 0)
			FlipSprites(flip, _sprites);

		if (_animatedSprites.Length > 0)
			FlipSprites(flip, _animatedSprites);

		if (_collisionShapes.Length > 0)
			FlipCollisionShapes(flip);

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
			collisionShape.Flip(flip);
	}
}
