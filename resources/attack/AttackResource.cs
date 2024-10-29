using Godot;

namespace Game.Resources.Attack;

[GlobalClass]
public partial class AttackResource : Resource
{

	[Export] public string AnimationName { get; private set; }
	[Export] public float AttackDamage { get; private set; }
	[Export] public PackedScene AttackScene { get; private set; }
}
