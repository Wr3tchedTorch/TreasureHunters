using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Game.Component;

[Icon("res://assets/godot icons/icon_gear.png")]
public partial class AnimationComponent : Node
{

	public string CurrentAnimation { get; private set; }

	[Export] private string _initialAnimation;
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export]
	private string[] AnimationNames
	{
		get => _animationNames.ToArray();
		set
		{
			_animationNames.Clear();
			foreach (var item in value)
			{
				_animationNames.Add(item);
			}
		}
	}

	private readonly HashSet<string> _animationNames = new();
	private Node2D _parent;

	public override void _Ready()
	{

		CurrentAnimation = _initialAnimation;

		_animatedSprite2D.Animation = "idle";
		_animatedSprite2D.Play();

		_parent = GetParent<Node2D>();
	}

	public override void _Process(double delta)
	{

		_animatedSprite2D.Play();
	}

	public void SetAnimation(string animationName)
	{

		if (animationName == CurrentAnimation || string.IsNullOrWhiteSpace(animationName) || !AnimationNames.Contains(animationName))
			return;

		CurrentAnimation = animationName;
		_animatedSprite2D.Animation = CurrentAnimation;
	}
}
