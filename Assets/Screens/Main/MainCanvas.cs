using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
	public Text scoreText;

	void Awake ()
	{
		GameStats.GlobalStats.scoreHud = scoreText;
	}
	
	void OnDestroy ()
	{
		GameStats.GlobalStats.scoreHud = null;
	}

}
