﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsCanvas : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnGoBack()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStates.Title);
	}
}
