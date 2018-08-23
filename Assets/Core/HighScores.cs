using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
	private static HighScores instance;
	public static HighScores Instance
	{
		get
		{
			return instance;
		}

	}

	public List<KeyValuePair<int, string>> highScoreList = new List<KeyValuePair<int, string>>();


	// Use this for initialization
	void Start ()
	{
		for (int i = 1; i <= 8; i++)
		{
			int score = PlayerPrefs.GetInt("hs_score" + i.ToString(), 10 - i);
			string name = PlayerPrefs.GetString("hs_name" + i.ToString(), "AAAAAAAA");

			highScoreList.Add(new KeyValuePair<int, string>(score, name));
		}

		instance = this;
	}
	
	public bool WouldScoreAppearOnHighScoreBoard(int score)
	{
		return (score > highScoreList[highScoreList.Count - 1].Key);
	}

	public bool TryAddToHighScoreBoard(int score, string name)
	{
		bool success = false;

		if (WouldScoreAppearOnHighScoreBoard(score))
		{
			int scorePosition = 0;
			for (int i = highScoreList.Count - 1; i >= 0; i--)
			{
				if (highScoreList[i].Key > score)
				{
					scorePosition = i + 1;
					break;
				}
			}

			highScoreList.Insert(scorePosition, new KeyValuePair<int, string>(score, name.Substring(0, Mathf.Min(8, name.Length))));
			SaveHighScores();
			success = true;
		}
		return success;
	}

	public void SaveHighScores()
	{
		for (int i = 0; i < highScoreList.Count; i++)
		{
			PlayerPrefs.SetInt("hs_score" + (i+1).ToString(), highScoreList[i].Key);
			PlayerPrefs.SetString("hs_name" + (i+1).ToString(), highScoreList[i].Value);
		}
		PlayerPrefs.Save();
	}
}
