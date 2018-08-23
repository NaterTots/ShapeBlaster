using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileLauncherFactory
{
	private static Dictionary<ProjectileLauncherType, ProjectileLauncherTypeDefinition> typeDefinitionMap = new Dictionary<ProjectileLauncherType, ProjectileLauncherTypeDefinition>();

	public static ProjectileLauncherTypeDefinition GetLauncherTypeDefinition(ProjectileLauncherType launcherType)
	{
		if (typeDefinitionMap.Count == 0)
		{
			GenerateTypeMap();
		}

		return typeDefinitionMap[launcherType];
	}

	private static void GenerateTypeMap()
	{
		//this is all throwaway code
		//this would be much better suited in data rather than hard-coded

		{
			var straightTypeDef = new ProjectileLauncherTypeDefinition();
			straightTypeDef.LauncherType = ProjectileLauncherType.Straight;
			straightTypeDef.ProjectileType = ProjectileType.Green;
			straightTypeDef.TimeBetweenShots = 0.25f;
			straightTypeDef.OnFireMethod = OnFireStraight;
			typeDefinitionMap.Add(straightTypeDef.LauncherType, straightTypeDef);
		}

		{
			var spreadTypeDef = new ProjectileLauncherTypeDefinition();
			spreadTypeDef.LauncherType = ProjectileLauncherType.Spread;
			spreadTypeDef.ProjectileType = ProjectileType.Blue;
			spreadTypeDef.TimeBetweenShots = 0.5f;
			spreadTypeDef.OnFireMethod = OnFireSpread;
			typeDefinitionMap.Add(spreadTypeDef.LauncherType, spreadTypeDef);
		}
		{
			var machineGunTypeDef = new ProjectileLauncherTypeDefinition();
			machineGunTypeDef.LauncherType = ProjectileLauncherType.MachineGun;
			machineGunTypeDef.ProjectileType = ProjectileType.Red;
			machineGunTypeDef.TimeBetweenShots = 0.1f;
			machineGunTypeDef.OnFireMethod = OnFireStraight;
			typeDefinitionMap.Add(machineGunTypeDef.LauncherType, machineGunTypeDef);
		}
		{
			var enemyBasicTypeDef = new ProjectileLauncherTypeDefinition();
			enemyBasicTypeDef.LauncherType = ProjectileLauncherType.EnemyBasic;
			enemyBasicTypeDef.ProjectileType = ProjectileType.Enemy;
			enemyBasicTypeDef.TimeBetweenShots = 0.5f;
			enemyBasicTypeDef.OnFireMethod = OnFireStraight;
			typeDefinitionMap.Add(enemyBasicTypeDef.LauncherType, enemyBasicTypeDef);
		}

		{
			var enemyRandomTypeDef = new ProjectileLauncherTypeDefinition();
			enemyRandomTypeDef.LauncherType = ProjectileLauncherType.EnemyRandom;
			enemyRandomTypeDef.ProjectileType = ProjectileType.Enemy;
			enemyRandomTypeDef.TimeBetweenShots = 0.25f;
			enemyRandomTypeDef.OnFireMethod = OnFireRandom;
			typeDefinitionMap.Add(enemyRandomTypeDef.LauncherType, enemyRandomTypeDef);
		}
	}

	static IEnumerable<Vector2> OnFireStraight(Vector2 direction)
	{
		yield return direction.normalized;
	}

	static IEnumerable<Vector2> OnFireSpread(Vector2 direction)
	{
		yield return direction.normalized;
		yield return Rotate(direction, -30f).normalized;
		yield return Rotate(direction, 30f).normalized;
	}

	static IEnumerable<Vector2> OnFireRandom(Vector2 direction)
	{
		yield return Rotate(direction, Random.Range(-30f, 30f)).normalized;
	}

	private static Vector2 Rotate(Vector2 v, float degrees)
	{
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
