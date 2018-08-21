using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this whole class could probably be more dynamic.
//creating 6 Image objects is silly, but for this simple game, it works
public class LivesHUD : MonoBehaviour
{
	public Image lives1;
	public Image lives2;
	public Image lives3;

	public Player player;

	void Start()
	{
		player.OnLivesChanged += OnPlayerLivesChanged;
	}

	public void SetLives(int lives)
	{
		lives1.gameObject.SetActive(lives > 0);
		lives2.gameObject.SetActive(lives > 1);
		lives3.gameObject.SetActive(lives > 2);
	}

	private void OnPlayerLivesChanged(int newLives)
	{
		SetLives(newLives);
	}
}
