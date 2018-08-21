using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelDefinition
{
	public int LevelNumber;

	[Serializable]
	public class EnemyLocation
	{
		public int type;
		public float xpos;
		public float ypos;

		public EnemyType GetEnemyType()
		{
			return (EnemyType)type;
		}

		public Vector2 GetLocation()
		{
			return new Vector2(xpos, ypos);
		}
	}

	public EnemyLocation[] MainWaveDefinition;
	public EnemyLocation[] BossWaveDefinition;
}

[Serializable]
public class LevelConfigurationData
{
	public LevelDefinition[] leveldefinitions;
}