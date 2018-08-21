using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
	private static ProjectileFactory theProjectileFactory;
	public static ProjectileFactory TheFactory 
	{
		get
		{
			return theProjectileFactory;
		}

	}

	public ObjectPool projectilePool;

	private Dictionary<ProjectileType, ProjectileTypeDefinition> typeDefinitionMap = new Dictionary<ProjectileType, ProjectileTypeDefinition>();

	public Sprite blueProjectile;
	public Sprite greenProjectile;
	public Sprite redProjectile;
	public Sprite enemyProjectile;

	// Use this for initialization
	void Start ()
	{

		projectilePool.Initialize();

		GenerateTypeMap();

		theProjectileFactory = this;
	}

	private void OnDestroy()
	{
		if (theProjectileFactory == this)
		{
			theProjectileFactory = null;
		}
	}

	public void SpawnNewProjectile(ProjectileType projectileType, Vector2 pos, Vector2 direction)
	{
		var newProjectile = projectilePool.InitNewObject();
		var typeDef = typeDefinitionMap[projectileType];
		newProjectile.GetComponent<Projectile>().Initialize(typeDef, pos, direction);
		newProjectile.layer = (int)typeDef.Layer;
	}

	private void GenerateTypeMap()
	{
		//this is all throwaway code
		//this would be much better suited in data rather than hard-coded
		if (typeDefinitionMap.Count == 0)
		{
			{
				var blueTypeDef = new ProjectileTypeDefinition();
				blueTypeDef.ProjectileType = ProjectileType.Blue;
				blueTypeDef.Sprite = blueProjectile;
				blueTypeDef.Speed = 10f;
				blueTypeDef.Damage = 1;
				blueTypeDef.Layer = ProjectileLayer.Player;
				typeDefinitionMap.Add(blueTypeDef.ProjectileType, blueTypeDef);
			}

			{
				var greenTypeDef = new ProjectileTypeDefinition();
				greenTypeDef.ProjectileType = ProjectileType.Green;
				greenTypeDef.Sprite = greenProjectile;
				greenTypeDef.Speed = 20f;
				greenTypeDef.Damage = 1;
				greenTypeDef.Layer = ProjectileLayer.Player;
				typeDefinitionMap.Add(greenTypeDef.ProjectileType, greenTypeDef);
			}

			{
				var redTypeDef = new ProjectileTypeDefinition();
				redTypeDef.ProjectileType = ProjectileType.Red;
				redTypeDef.Sprite = redProjectile;
				redTypeDef.Speed = 10f;
				redTypeDef.Damage = 1;
				redTypeDef.Layer = ProjectileLayer.Player;
				typeDefinitionMap.Add(redTypeDef.ProjectileType, redTypeDef);
			}

			{
				var enemyTypeDef = new ProjectileTypeDefinition();
				enemyTypeDef.ProjectileType = ProjectileType.Enemy;
				enemyTypeDef.Sprite = enemyProjectile;
				enemyTypeDef.Speed = 10f;
				enemyTypeDef.Damage = 1;
				enemyTypeDef.Layer = ProjectileLayer.Enemy;
				typeDefinitionMap.Add(enemyTypeDef.ProjectileType, enemyTypeDef);
			}
		}
	}
}
