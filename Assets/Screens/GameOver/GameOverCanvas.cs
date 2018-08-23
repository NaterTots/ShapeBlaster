using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour 
{
	public Text finalScore;
	public InputField inputField;

	// Use this for initialization
	void Start () 
	{
		finalScore.text = GameStats.GlobalStats.Score.ToString().PadLeft(8, '0');
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnSubmit()
	{
		HighScores.Instance.TryAddToHighScoreBoard(GameStats.GlobalStats.Score, inputField.text);
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.HighScore);
	}
}
