using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTypeDefinition
{
	public ProjectileType ProjectileType { get; set; }
	public Sprite Sprite { get; set; }
	public float Speed { get; set; }
	public int Damage { get; set; }
	public ProjectileLayer Layer { get; set; }
}

public enum ProjectileType
{
	Blue,
	Green,
	Red,
	Enemy
}

public enum ProjectileLayer
{
	Enemy = 8,
	Player = 9
}
