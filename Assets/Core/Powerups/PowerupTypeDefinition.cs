using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCollect();

public class PowerupTypeDefinition
{
	public PowerupType PowerupType { get; set; }
	public Sprite Sprite { get; set; }
	public OnCollect OnCollectMethod { get; set; }
	public int OddsOfSpawning { get; set; } //this is a number 1-100 that's a percentage
}

public enum PowerupType
{
	Health,
	ExtraLife,
	SpreadGun,
	MachineGun
}
