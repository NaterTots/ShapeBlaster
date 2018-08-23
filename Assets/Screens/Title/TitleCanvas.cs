using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvas : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnGoToGame()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.Main);
	}

	public void OnGoToCredits()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.Credits);
	}

	public void OnGoToHighScore()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.HighScore);
	}
}
