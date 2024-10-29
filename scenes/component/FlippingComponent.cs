using System.Linq;
using Game.Collision;
using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class FlippingComponent : Node
{

	public bool IsParentFacingLeft;

	[Export] private bool _isSpriteLeftOriented;
	[Export] public AnimatedSprite2D[] AnimatedSprites;
	[Export] public Sprite2D[] Sprites;
	[Export] public CollisionShape2D[] CollisionShapes;
	[Export] public CollisionPolygon2D[] CollisionPolygons;


	public void FlipH(bool flip)
	{

		IsParentFacingLeft = flip;

		if (Sprites.Length > 0)
			FlipSprites(flip, Sprites);

		if (AnimatedSprites.Length > 0)
			FlipSprites(flip, AnimatedSprites);

		if (CollisionShapes.Length > 0)
			FlipCollisionShapes(flip);

		if (CollisionPolygons.Length > 0)
			FlipCollisionPolygons(flip);
	}

	private void FlipCollisionPolygons(bool flip)
	{
		foreach (CollisionPolygon2D polygon in CollisionPolygons)
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
		foreach (CollisionFlipper collisionShape in CollisionShapes.Cast<CollisionFlipper>())
			collisionShape.Flip(flip);
	}
}
