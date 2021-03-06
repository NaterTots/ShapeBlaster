﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSceneController : MonoBehaviour , IPersistedController
{
	public string currentScene;

	public Dictionary<GameStates, string> gameStateSceneMap = new Dictionary<GameStates, string>();

	private bool transitioningScenes = false;

	void Awake()
	{
		InstantiateGameStateSceneMap();

		GameController.AddController(this);
	}

	void Start()
	{
		GameController.GetController<GameStateEngine>().AddAllStateListener(OnGameStateChange);
	}

	void OnGameStateChange()
	{
		string nextScene = string.Empty;

		if (gameStateSceneMap.ContainsKey(GameController.GetController<GameStateEngine>().CurrentState))
		{
			nextScene = gameStateSceneMap[GameController.GetController<GameStateEngine>().CurrentState];
		}
		else
		{
			Debug.LogError("State does not have a defined scene: " + GameController.GetController<GameStateEngine>().CurrentState.ToString());
		}
		

		if (currentScene != string.Empty)
		{
			SceneManager.UnloadSceneAsync(currentScene);
		}

		if (nextScene != string.Empty)
		{
			SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
			currentScene = nextScene;
			transitioningScenes = true;
			
		}
	}

	//this is required because LoadScene triggers the scene to be loaded in the next cycle
	//so you can't call LoadScene and immediately call SetActiveScene
	void Update()
	{
		if (transitioningScenes)
		{
			Scene s = SceneManager.GetSceneByName(currentScene);
			if (s.isLoaded)
			{
				SceneManager.SetActiveScene(s);
				transitioningScenes = false;
			}
			
		}
	}

	private void InstantiateGameStateSceneMap()
	{
		int stateCount = Enum.GetNames(typeof(GameStates)).Length;
		for (int i = 0; i < stateCount; ++i)
		{
			var memInfo = typeof(GameStates).GetMember(((GameStates)i).ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(StateSceneAttribute), false);
			if (attributes.Length > 0)
			{
				gameStateSceneMap.Add((GameStates)i, ((StateSceneAttribute)attributes[0]).SceneName);
			}
			else
			{
				Debug.LogError("State does not have a defined scene: " + GameController.GetController<GameStateEngine>().CurrentState.ToString());
			}
		}	
	}
}