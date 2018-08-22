using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerable<Vector2> OnFire(Vector2 direction);

public class ProjectileLauncherTypeDefinition
{
	public ProjectileLauncherType LauncherType { get; set; }
	public float TimeBetweenShots { get; set; }
	public ProjectileType ProjectileType { get; set; }
	public OnFire OnFireMethod { get; set; }
}

public enum ProjectileLauncherType
{
	Straight,
	Spread,
	MachineGun,
	EnemyBasic,
	EnemyRandom
}
