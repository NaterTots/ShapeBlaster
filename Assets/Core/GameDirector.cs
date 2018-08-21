using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameDirector : MonoBehaviour
{
	public EnemyFactory enemyFactory;

	private InputController inputController;

	float enemyTimer = 0.0f;
	const float timeBetweenEnemies = 3.0f;

	const float timeToSpawnAllEnemies = 2.0f;

	public GameObject levelUI;
	public Text levelUIText;
	public Text levelSubText;

	public Player thePlayer;

	private LevelDefinition currentLevelDefinition;
	private bool onBossWave = false;

	// Use this for initialization
	void Start()
	{
		enemyTimer = timeBetweenEnemies;

		inputController = GameController.GetController<InputController>();

		thePlayer.OnPlayerDead += OnPlayerDead;

		StartLevel(1);
	}

	// Update is called once per frame
	void Update()
	{
		//enemyTimer -= Time.deltaTime;
		//if (enemyTimer <= 0.0f)
		//{
		//	enemyTimer = timeBetweenEnemies;
		//	enemyFactory.SpawnNewEnemy(GetRandomEnemyType(),
		//		new Vector2(Random.Range(-4.5f, 7.5f), Random.Range(0f, 8.5f)));
		//}

		if (isSpawningEnemies)
		{
			UpdateSpawnEnemies();
		}

		if (awaitingPressFire)
		{
			if (inputController.IsFirePressed())
			{
				RemoveLevelUI();

				//TODO: createa a small level UI

				thePlayer.ToggleShootingControl(true);
				enemyFactory.StartMoveAndShootOfAllEnemies();

				awaitingPressFire = false;
			}
		}
	}

	EnemyType GetRandomEnemyType()
	{
		var enemyTypeRandomizer = Random.Range(0.0f, 1.0f);
		if (enemyTypeRandomizer < 0.33f)
		{
			return EnemyType.Cross;
		}
		else if (enemyTypeRandomizer < 0.66f)
		{
			return EnemyType.Square;
		}
		else
		{
			return EnemyType.Triangle;
		}
	}

	private void DisplayLevelUIWithoutSubtext(int levelNum)
	{
		string levelText = levelNum.ToString();
		DisplayLevelUIWithoutSubtext(levelText.PadLeft(2, '0'));
	}

	private void DisplayLevelUIWithoutSubtext(string text)
	{
		levelUIText.text = text;
		levelUI.SetActive(true);
		levelSubText.gameObject.SetActive(false);
	}

	private bool awaitingPressFire = false;

	private void DisplayLevelUISubText()
	{
		levelSubText.gameObject.SetActive(true);
		awaitingPressFire = true;
	}

	private void RemoveLevelUI()
	{
		levelUI.SetActive(false);
	}

	private void StartLevel(int levelNum)
	{
		thePlayer.ToggleShootingControl(false);
		DisplayLevelUIWithoutSubtext(levelNum);

		currentLevelDefinition = LevelManager.Instance.GetLevelDefinition(levelNum);
		enemySpawnOrder = currentLevelDefinition.MainWaveDefinition.OrderBy(x => Random.value).ToArray();
		enemySpawnIndex = 0;
		isSpawningEnemies = true;
		timeBetweenEnemySpawns =  timeToSpawnAllEnemies / (float)(enemySpawnOrder.Length - 1);
		nextEnemyTimer = 0.0f;
	}

	private void StartBossWave()
	{
		thePlayer.ToggleShootingControl(false);
		foreach(var enemy in currentLevelDefinition.BossWaveDefinition)
		{
			enemyFactory.SpawnNewEnemy(enemy.GetEnemyType(), enemy.GetLocation());
		}
	}

	bool isSpawningEnemies = false;
	LevelDefinition.EnemyLocation[] enemySpawnOrder;
	int enemySpawnIndex = 0;
	float timeBetweenEnemySpawns = 0.0f;
	float nextEnemyTimer = 0.0f;

	private void UpdateSpawnEnemies()
	{
		nextEnemyTimer += Time.deltaTime;

		if (nextEnemyTimer >= timeBetweenEnemySpawns)
		{
			//SPAWN NEXT ENEMY
			var nextEnemy = enemySpawnOrder[enemySpawnIndex];
			enemyFactory.SpawnNewEnemy(nextEnemy.GetEnemyType(), nextEnemy.GetLocation());
			nextEnemyTimer = 0.0f;
			enemySpawnIndex++;
			if (enemySpawnIndex >= enemySpawnOrder.Length)
			{
				isSpawningEnemies = false;
				DisplayLevelUISubText();
			}
		}
	}

	bool isInWave = false;
	float timeSinceLastWaveCheck = 0.0f;

	private void UpdateCheckWave()
	{
		timeSinceLastWaveCheck += Time.deltaTime;
		if (timeSinceLastWaveCheck > 1.0f)
		{
			if (!enemyFactory.EnemiesStillAlive())
			{
				if (onBossWave)
				{
					//transition to next level
					int nextLevel = currentLevelDefinition.LevelNumber + 1;
					if (LevelManager.Instance.DoesLevelExist(nextLevel))
					{
						StartLevel(nextLevel);
					}
					else
					{
						DisplayLevelUIWithoutSubtext("beat the game");
						StartCoroutine(WaitToDisplayUISubtext());
					}
				}
				else
				{
					//transition to boss wave
					StartBossWave();
				}
			}

			timeSinceLastWaveCheck = 0.0f;
		}
	}

	private void OnPlayerDead()
	{
		DisplayLevelUIWithoutSubtext("game over");
		StartCoroutine(WaitToDisplayUISubtext());
	}

	IEnumerator WaitToDisplayUISubtext()
	{
		yield return new WaitForSeconds(3);
		DisplayLevelUISubText();
	}
}
