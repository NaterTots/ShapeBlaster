using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this whole class could probably be more dynamic.
//creating 6 Image objects is silly, but for this simple game, it works
public class PlayerHealth : MonoBehaviour
{
	public Image health1;
	public Image health2;
	public Image health3;
	public Image health4;
	public Image health5;
	public Image health6;

	public Sprite emptyTile;
	public Sprite fullTile;

	public Player player;

	void Start()
	{
		player.OnHealthChanged += OnPlayerHealthChanged;
	}

	public void SetHealth(int health)
	{
		health1.sprite = (health > 0 ? fullTile : emptyTile);
		health2.sprite = (health > 1 ? fullTile : emptyTile);
		health3.sprite = (health > 2 ? fullTile : emptyTile);
		health4.sprite = (health > 3 ? fullTile : emptyTile);
		health5.sprite = (health > 4 ? fullTile : emptyTile);
		health6.sprite = (health > 5 ? fullTile : emptyTile);
	}

	private void OnPlayerHealthChanged(int newHealth)
	{
		SetHealth(newHealth);
	}
}
