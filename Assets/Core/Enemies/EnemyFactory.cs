using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
	public ObjectPool enemyPool;

	private Dictionary<EnemyType, EnemyTypeDefinition> typeDefinitionMap = new Dictionary<EnemyType, EnemyTypeDefinition>();

	public Sprite crossSprite;
	public Sprite squareSprite;
	public Sprite triangleSprite;

	// Use this for initialization
	void Start()
	{
		enemyPool.Initialize();

		GenerateTypeMap();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public Enemy SpawnNewEnemy(EnemyType enemyType, Vector2 pos)
	{
		var newEnemy = enemyPool.InitNewObject();
		var enemy = newEnemy.GetComponent<Enemy>();
		enemy.Initialize(typeDefinitionMap[enemyType], pos);
		return enemy;
	}

	/// <summary>
	/// This is an expensive call, don't call it in a tight loop
	/// </summary>
	/// <returns></returns>
	public bool EnemiesStillAlive()
	{
		return (enemyPool.ActiveCount > 0);
	}

	public void StopMoveAndShootOfAllEnemies()
	{
		enemyPool.SendMessageToAllActive("StopMoveAndShoot");
	}

	public void StartMoveAndShootOfAllEnemies()
	{
		enemyPool.SendMessageToAllActive("StartMoveAndShoot");
	}

	private void GenerateTypeMap()
	{
		//this is all throwaway code
		//this would be much better suited in data rather than hard-coded
		if (typeDefinitionMap.Count == 0)
		{
			{
				var crossTypeDef = new EnemyTypeDefinition();
				crossTypeDef.EnemyType = EnemyType.Cross;
				crossTypeDef.Sprite = crossSprite;
				crossTypeDef.StartingHealth = 1;
				crossTypeDef.LauncherType = ProjectileLauncherType.EnemyBasic;
				crossTypeDef.PointsForKilling = 25;
				typeDefinitionMap.Add(EnemyType.Cross, crossTypeDef);
			}

			{
				var squareTypeDef = new EnemyTypeDefinition();
				squareTypeDef.EnemyType = EnemyType.Square;
				squareTypeDef.Sprite = squareSprite;
				squareTypeDef.StartingHealth = 1;
				squareTypeDef.LauncherType = ProjectileLauncherType.EnemyBasic;
				squareTypeDef.PointsForKilling = 50;
				typeDefinitionMap.Add(EnemyType.Square, squareTypeDef);
			}

			{
				var triangleTypeDef = new EnemyTypeDefinition();
				triangleTypeDef.EnemyType = EnemyType.Triangle;
				triangleTypeDef.Sprite = triangleSprite;
				triangleTypeDef.StartingHealth = 2;
				triangleTypeDef.LauncherType = ProjectileLauncherType.EnemyRandom;
				triangleTypeDef.PointsForKilling = 100;
				typeDefinitionMap.Add(EnemyType.Triangle, triangleTypeDef);
			}
		}
	}
}
