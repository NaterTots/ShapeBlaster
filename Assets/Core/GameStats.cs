using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
	private static GameStats theGameStats;
	public static GameStats GlobalStats
	{
		get
		{
			return theGameStats;
		}

	}

	public int Score { get; set; }

	//it's weird to have this here instead of having the HUD subscribe to changes, but...this is the easy solution
	public Text scoreHud;

	// Use this for initialization
	void Start ()
	{
		theGameStats = this;
	}

	private void OnDestroy()
	{
		if (theGameStats == this)
		{
			theGameStats = null;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SetScore(int newScore)
	{
		Score = newScore;
		string scoreAsText = Score.ToString();
		scoreHud.text = scoreAsText.PadLeft(6, '0');
	}

	public void ChangeScore(int diff)
	{
		SetScore(Score + diff);
	}
}
