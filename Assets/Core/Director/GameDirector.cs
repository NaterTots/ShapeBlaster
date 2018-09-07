using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameDirector : MonoBehaviour
{
	public EnemyFactory enemyFactory;
	public Player thePlayer;

	public GameObject levelUI;
	public Text levelUIText;
	public Text levelSubText;

	public enum DirectorState
	{
		LoadingLevel,
		AwaitingLevelStart,
		InMainWave,
		LoadingBossWave,
		AwaitingBossWaveStart,
		InBossWave,
		GameOverLoss,
		GameOverWin
	}
	private DirectorState directorState;

	private InputController inputController;

	private int currentLevelNum = 1;
	private LevelDefinition currentLevelDefinition;

	private static GameDirector instance;
	public static GameDirector Instance
	{
		get
		{
			return instance;
		}

	}

	// Use this for initialization
	void Start()
	{
		inputController = GameController.GetController<InputController>();
		thePlayer.OnPlayerDead += OnPlayerDead;
		GameStats.GlobalStats.SetScore(0);
		TransitionToState(DirectorState.LoadingLevel);
		instance = this;
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	void TransitionToState(DirectorState newState)
	{
		directorState = newState;

		switch(directorState)
		{
			case DirectorState.LoadingLevel:
				StartLoadingLevel();
				break;
			case DirectorState.AwaitingLevelStart:
				StartAwaitingLevelStart();
				break;
			case DirectorState.InMainWave:
				StartInMainWave();
				break;
			case DirectorState.LoadingBossWave:
				StartLoadingBossWave();
				break;
			case DirectorState.AwaitingBossWaveStart:
				StartAwaitingBossWaveStart();
				break;
			case DirectorState.InBossWave:
				StartInBossWave();
				break;
			case DirectorState.GameOverLoss:
				StartGameOverLoss();
				break;
			case DirectorState.GameOverWin:
				StartGameOverWin();
				break;
		}
	}

	// Update is called once per frame
	void Update()
	{
		switch (directorState)
		{
			case DirectorState.LoadingLevel:
				UpdateLoadingLevel();
				break;
			case DirectorState.AwaitingLevelStart:
				UpdateAwaitingLevelStart();
				break;
			case DirectorState.InMainWave:
				UpdateInMainWave();
				break;
			case DirectorState.LoadingBossWave:
				UpdateLoadingBossWave();
				break;
			case DirectorState.AwaitingBossWaveStart:
				UpdateAwaitingBossWaveStart();
				break;
			case DirectorState.InBossWave:
				UpdateInBossWave();
				break;
			case DirectorState.GameOverLoss:
				UpdateGameOverLoss();
				break;
			case DirectorState.GameOverWin:
				UpdateGameOverWin();
				break;
		}
	}

	#region LoadingLevel

	const float timeToSpawnAllEnemies = 2.0f;

	LevelDefinition.EnemyLocation[] enemySpawnOrder;
	int enemySpawnIndex = 0;
	float timeBetweenEnemySpawns = 0.0f;
	float nextEnemyTimer = 0.0f;

	void StartLoadingLevel()
	{
		thePlayer.ToggleShootingControl(false);
		DisplayLevelUIWithoutSubtext(currentLevelNum);

		currentLevelDefinition = LevelManager.Instance.GetLevelDefinition(currentLevelNum);
		enemySpawnOrder = currentLevelDefinition.MainWaveDefinition.OrderBy(x => Random.value).ToArray();
		enemySpawnIndex = 0;
		if (enemySpawnOrder.Length > 1)
		{
			timeBetweenEnemySpawns = timeToSpawnAllEnemies / (float)(enemySpawnOrder.Length - 1);
		}
		nextEnemyTimer = 0.0f;
	}

	void UpdateLoadingLevel()
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
				TransitionToState(DirectorState.AwaitingLevelStart);			
			}
		}
	}

	#endregion

	#region AwaitingLevelStart

	void StartAwaitingLevelStart()
	{
		thePlayer.ToggleShootingControl(false);
		DisplayLevelUISubText();
	}

	void UpdateAwaitingLevelStart()
	{
		if (inputController.IsFirePressed())
		{
			RemoveLevelUI();

			//TODO: createa a small level UI
			TransitionToState(DirectorState.InMainWave);
		}
	}

	#endregion

	#region InMainWave

	float timeSinceLastWaveCheck = 0.0f;

	void StartInMainWave()
	{
		thePlayer.ToggleShootingControl(true);
		enemyFactory.StartMoveAndShootOfAllEnemies();

		timeSinceLastWaveCheck = 0.0f;
	}

	void UpdateInMainWave()
	{
		timeSinceLastWaveCheck += Time.deltaTime;
		if (timeSinceLastWaveCheck > 1.0f)
		{
			if (!enemyFactory.EnemiesStillAlive())
			{
				TransitionToState(DirectorState.LoadingBossWave);
			}

			timeSinceLastWaveCheck = 0.0f;
		}
	}

	#endregion

	#region LoadingBossWave

	void StartLoadingBossWave()
	{
		thePlayer.ToggleShootingControl(false);
		ProjectileFactory.TheFactory.KillAllProjectiles();

		DisplayLevelUIWithoutSubtext("BOSS");
		foreach (var enemy in currentLevelDefinition.BossWaveDefinition)
		{
			enemyFactory.SpawnNewEnemy(enemy.GetEnemyType(), enemy.GetLocation());
		}

		bossLoadingTimer = 0.0f;
	}

	const float bossLoadTime = 2.0f;
	float bossLoadingTimer = 0.0f;

	void UpdateLoadingBossWave()
	{
		bossLoadingTimer += Time.deltaTime;

		if (bossLoadingTimer > bossLoadTime)
		{
			TransitionToState(DirectorState.AwaitingBossWaveStart);
		}
	}

	#endregion

	#region AwaitingBossWaveStart

	void StartAwaitingBossWaveStart()
	{
		thePlayer.ToggleShootingControl(false);
		DisplayLevelUISubText();
	}

	void UpdateAwaitingBossWaveStart()
	{
		if (inputController.IsFirePressed())
		{
			RemoveLevelUI();

			TransitionToState(DirectorState.InBossWave);
		}
	}

	#endregion

	#region InBossWave

	void StartInBossWave()
	{
		thePlayer.ToggleShootingControl(true);
		enemyFactory.StartMoveAndShootOfAllEnemies();

		timeSinceLastWaveCheck = 0.0f;
	}

	void UpdateInBossWave()
	{
		timeSinceLastWaveCheck += Time.deltaTime;
		if (timeSinceLastWaveCheck > 1.0f)
		{
			if (!enemyFactory.EnemiesStillAlive())
			{
				//transition to next level
				currentLevelNum++;
				if (LevelManager.Instance.DoesLevelExist(currentLevelNum))
				{
					TransitionToState(DirectorState.LoadingLevel);
				}
				else
				{
					TransitionToState(DirectorState.GameOverWin);
				}
			}

			timeSinceLastWaveCheck = 0.0f;
		}
	}

	#endregion

	#region GameOverLoss

	bool readyForEndOfGame = false;

	void StartGameOverLoss()
	{
		enemyFactory.StartMoveAndShootOfAllEnemies();
		ProjectileFactory.TheFactory.KillAllProjectiles();

		DisplayLevelUIWithoutSubtext("game over");
		readyForEndOfGame = false;
		StartCoroutine(WaitToDisplayUISubtext());
	}

	void UpdateGameOverLoss()
	{
		UpdateEndOfGame();
	}

	#endregion

	#region GameOverWin

	void StartGameOverWin()
	{
		ProjectileFactory.TheFactory.KillAllProjectiles();

		DisplayLevelUIWithoutSubtext("beat the game");
		readyForEndOfGame = false;
		StartCoroutine(WaitToDisplayUISubtext());
	}

	void UpdateGameOverWin()
	{
		UpdateEndOfGame();
	}

	#endregion

	#region Helpers

	private void UpdateEndOfGame()
	{
		if (readyForEndOfGame && inputController.IsFirePressed())
		{
			if (HighScores.Instance.WouldScoreAppearOnHighScoreBoard(GameStats.GlobalStats.Score))
			{
				GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.GameOver);
			}
			else
			{
				GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.HighScore);
			}
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

	private void DisplayLevelUISubText()
	{
		levelSubText.gameObject.SetActive(true);
		readyForEndOfGame = true;
	}

	private void RemoveLevelUI()
	{
		levelUI.SetActive(false);
	}

	private void OnPlayerDead()
	{
		TransitionToState(DirectorState.GameOverLoss);
	}

	IEnumerator WaitToDisplayUISubtext()
	{
		yield return new WaitForSeconds(3);
		DisplayLevelUISubText();
	}

	#endregion
}
