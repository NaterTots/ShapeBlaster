
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSystem : MonoBehaviour
{
	public ObjectPool powerupPool;

	private List<KeyValuePair<float,PowerupTypeDefinition>> typeDefinitionMap = new List<KeyValuePair<float, PowerupTypeDefinition>>();

	public Sprite extraLifeSprite;
	public Sprite healthSprite;
	public Sprite machineGunSprite;
	public Sprite spreadGunSprite;

	private static PowerupSystem instance;
	public static PowerupSystem Instance
	{
		get
		{
			return instance;
		}

	}

	// Use this for initialization
	void Start ()
	{
		powerupPool.Initialize();
		GenerateTypeMap();
		instance = this;
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public void EnemyDied(Vector2 location)
	{
		float powerupOdds = Random.value;

		foreach (var powerup in typeDefinitionMap)
		{
			if (powerup.Key > powerupOdds)
			{
				var newPowerup = powerupPool.InitNewObject();
				newPowerup.GetComponent<Powerup>().Initialize(powerup.Value, location);
				break;
			}
		}
	}

	private void GenerateTypeMap()
	{
		//this is all throwaway code
		//this would be much better suited in data rather than hard-coded
		if (typeDefinitionMap.Count == 0)
		{
			float runningOdds = 0.0f;
			{
				var powerup = new PowerupTypeDefinition();
				powerup.PowerupType = PowerupType.Health;
				powerup.Sprite = healthSprite;
				powerup.OnCollectMethod = OnCollectHealth;
				powerup.OddsOfSpawning = 15;
				runningOdds += (powerup.OddsOfSpawning / 100.0f);
				typeDefinitionMap.Add(new KeyValuePair<float, PowerupTypeDefinition>(runningOdds, powerup));
			}
			{
				var powerup = new PowerupTypeDefinition();
				powerup.PowerupType = PowerupType.ExtraLife;
				powerup.Sprite = extraLifeSprite;
				powerup.OnCollectMethod = OnExtraLife;
				powerup.OddsOfSpawning = 6;
				runningOdds += (powerup.OddsOfSpawning / 100.0f);
				typeDefinitionMap.Add(new KeyValuePair<float, PowerupTypeDefinition>(runningOdds, powerup));				
			}
			{
				var powerup = new PowerupTypeDefinition();
				powerup.PowerupType = PowerupType.MachineGun;
				powerup.Sprite = machineGunSprite;
				powerup.OnCollectMethod = OnCollectMachineGun;
				powerup.OddsOfSpawning = 10;
				runningOdds += (powerup.OddsOfSpawning / 100.0f);
				typeDefinitionMap.Add(new KeyValuePair<float, PowerupTypeDefinition>(runningOdds, powerup));
			}
			{
				var powerup = new PowerupTypeDefinition();
				powerup.PowerupType = PowerupType.SpreadGun;
				powerup.Sprite = spreadGunSprite;
				powerup.OnCollectMethod = OnCollectSpreadGun;
				powerup.OddsOfSpawning = 10;
				runningOdds += (powerup.OddsOfSpawning / 100.0f);
				typeDefinitionMap.Add(new KeyValuePair<float, PowerupTypeDefinition>(runningOdds, powerup));
			}
		}
	}

	void OnExtraLife()
	{
		GameDirector.Instance.thePlayer.AddLives(1);
	}

	void OnCollectHealth()
	{
		GameDirector.Instance.thePlayer.ChangeHealth(3);
	}

	void OnCollectMachineGun()
	{
		GameDirector.Instance.thePlayer.SetGunAsMachineGun();
	}

	void OnCollectSpreadGun()
	{
		GameDirector.Instance.thePlayer.SetGunAsSpread();
	}
}
