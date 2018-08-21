using UnityEngine;

public class EnemyTypeDefinition
{
	public EnemyType EnemyType { get; set; }
	public Sprite Sprite { get; set; }
	public int StartingHealth { get; set; }
	public ProjectileLauncherType LauncherType { get; set; }
	public int PointsForKilling { get; set; }
}

public enum EnemyType
{
	Square = 0,
	Triangle = 1,
	Cross = 2
}
