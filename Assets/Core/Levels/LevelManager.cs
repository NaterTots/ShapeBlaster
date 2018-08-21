using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private static LevelManager instance;
	public static LevelManager Instance
	{
		get
		{
			return instance;
		}

	}

	public TextAsset dataFile;

	Dictionary<int, LevelDefinition> _levels = new Dictionary<int, LevelDefinition>();

	// Use this for initialization
	void Start ()
	{
		instance = this;

		LevelConfigurationData configData = JsonUtility.FromJson<LevelConfigurationData>(dataFile.text);

		foreach(var level in configData.leveldefinitions)
		{
			_levels.Add(level.LevelNumber, level);
		}

	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public LevelDefinition GetLevelDefinition(int levelNum)
	{
		return _levels[levelNum];
	}

	public bool DoesLevelExist(int levelNum)
	{
		return (_levels.ContainsKey(levelNum));
	}
}
