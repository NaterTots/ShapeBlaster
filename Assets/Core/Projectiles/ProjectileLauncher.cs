using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
	public ProjectileLauncherTypeDefinition TypeDefinition
	{
		get;
		private set;
	}

	public Vector2 Direction { get; private set; }

	float timeSinceLastShot = 0.0f;


	public void Initialize(ProjectileLauncherTypeDefinition typeDef, Vector2 direction)
	{
		TypeDefinition = typeDef;
		timeSinceLastShot = 0.0f;

		Direction = direction;
	}

	public void SetTypeDefinition(ProjectileLauncherTypeDefinition typeDef)
	{
		TypeDefinition = typeDef;
	}

	// Update is called once per frame
	void Update ()
	{
		timeSinceLastShot += Time.deltaTime;
	}

	public void OnFire()
	{
		if (timeSinceLastShot > TypeDefinition.TimeBetweenShots)
		{
			foreach(Vector2 direction in TypeDefinition.OnFireMethod(Direction))
			{
				ProjectileFactory.TheFactory.SpawnNewProjectile(TypeDefinition.ProjectileType, transform.position, direction);
			}

			timeSinceLastShot = 0.0f;
		}
	}
}
