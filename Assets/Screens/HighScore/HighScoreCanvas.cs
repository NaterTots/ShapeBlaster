using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreCanvas : MonoBehaviour 
{
	public Text scoreBoard;

	// Use this for initialization
	void Start () 
	{
		var highScoreList = HighScores.Instance.highScoreList;

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < highScoreList.Count; i++)
		{
			sb.AppendLine((i + 1).ToString() + ". " + highScoreList[i].Value.PadRight(8, ' ') + "     " + highScoreList[i].Key.ToString().PadLeft(8, '0'));
		}

		scoreBoard.text = sb.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnGoToTitle()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.Title);
	}
}
