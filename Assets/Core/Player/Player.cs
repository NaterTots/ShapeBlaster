using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
	private InputController inputController;
	private Rigidbody2D rigidBody;
	public float speed = 10.0f;
	public int maxHealth = 6;
	public int maxLives = 3;

	public Sprite blueSprite;
	public Sprite greenSprite;
	public Sprite orangeSprite;
	public Sprite redSprite;

	ProjectileLauncher projectileLauncher;

	public int Health { get; set; }
	public int Lives { get; set; }

	public ProjectileLauncherType startingLauncher;

	private bool _canShoot = true;

	// Use this for initialization
	void Start()
	{
		inputController = GameController.GetController<InputController>();

		rigidBody = GetComponent<Rigidbody2D>();

		projectileLauncher = GetComponentInChildren<ProjectileLauncher>();
		projectileLauncher.Initialize(ProjectileLauncherFactory.GetLauncherTypeDefinition(startingLauncher), Vector2.up);
		GetComponent<SpriteRenderer>().sprite = greenSprite;

		inputController.AddKeyCodeListener(KeyCode.Space, OnFire);

		SetHealth(maxHealth); //hard coded full health (because that's how many health bars with in the UI)
		SetLives(maxLives);
	}

	// Update is called once per frame
	void Update()
	{
		var vel = rigidBody.velocity;
		float h = inputController.GetAxis(InputController.Axis.Horizontal);
		vel.x = (h == 0 ? 0.0f : ((h > 0) ? speed : -speed));
		rigidBody.velocity = vel;
	}

	void OnFire()
	{
		if (_canShoot)
		{
			projectileLauncher.OnFire();
		}
	}

	public void ToggleShootingControl(bool canShoot)
	{
		_canShoot = canShoot;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (invincible) return;

		var projectile = collider.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			ChangeHealth(-projectile.Damage);
			projectile.OnHitSomething();
			GameController.GetController<SoundController>().PlaySoundEffect(SoundController.SoundEffect.PlayerHit);
		}

		var powerup = collider.gameObject.GetComponent<Powerup>();
		if (powerup != null)
		{
			powerup.TypeDefinition.OnCollectMethod();
			powerup.OnHitSomething();
			GameController.GetController<SoundController>().PlaySoundEffect(SoundController.SoundEffect.PickupPowerup);
		}
	}

	public void ChangeHealth(int diff)
	{
		SetHealth(Health + diff);
	}

	private void SetHealth(int health)
	{
		Health = health;
		Health = Mathf.Min(Health, maxHealth);
		if (OnHealthChanged != null)
		{
			OnHealthChanged(Health);
		}

		if (Health <= 0)
		{
			OnDied();
		}
	}

	#region Lives

	private void SetLives(int lives)
	{
		Lives = lives;
		Lives = Mathf.Min(Lives, maxLives);
		if (OnLivesChanged != null)
		{
			OnLivesChanged(Lives);
		}

		if (Lives < 0)
		{
			//GAME OVER
			if (OnPlayerDead != null)
			{
				OnPlayerDead();
			}

			ToggleShootingControl(false);
			GameController.GetController<SoundController>().PlaySoundEffect(SoundController.SoundEffect.LostGame);
			gameObject.SetActive(false);
		}
	}

	public void AddLives(int lives)
	{
		SetLives(Lives + lives);
	}

	#endregion Lives

	#region Guns

	public void SetGunAsMachineGun()
	{
		projectileLauncher.SetTypeDefinition(ProjectileLauncherFactory.GetLauncherTypeDefinition(ProjectileLauncherType.MachineGun));
		GetComponent<SpriteRenderer>().sprite = redSprite;
	}

	public void SetGunAsSpread()
	{
		projectileLauncher.SetTypeDefinition(ProjectileLauncherFactory.GetLauncherTypeDefinition(ProjectileLauncherType.Spread));
		GetComponent<SpriteRenderer>().sprite = blueSprite;
	}

	#endregion Guns

	private bool invincible = false;

	private void OnDied()
	{
		SetLives(Lives - 1);
		SetHealth(maxHealth);
		SetInvincibility();
	}

	private void SetInvincibility()
	{
		invincible = true;
		ToggleShootingControl(false);
		GetComponent<Animator>().Play("PlayerDead");
		StartCoroutine(WaitAndRemoveInvincibility());
	}

	private IEnumerator WaitAndRemoveInvincibility()
	{
		yield return new WaitForSeconds(3.0f);
		invincible = false;
		ToggleShootingControl(true);
		GetComponent<Animator>().Play("Idle");
	}

	public delegate void HealthChanged(int newHealth);
	public HealthChanged OnHealthChanged;

	public delegate void LivesChanged(int newLives);
	public LivesChanged OnLivesChanged;

	public delegate void PlayerDied();
	public PlayerDied OnPlayerDead;
}
