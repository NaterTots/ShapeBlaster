using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private InputController inputController;
	private Rigidbody2D rigidBody;
	public float speed = 10.0f;
	public int maxHealth = 6;
	public int maxLives = 3;

	public ProjectileLauncherType launcherType;

	ProjectileLauncher projectileLauncher;

	public int Health { get; set; }
	public int Lives { get; set; }

	private bool _canShoot = true;

	// Use this for initialization
	void Start()
	{
		inputController = GameController.GetController<InputController>();

		rigidBody = GetComponent<Rigidbody2D>();

		projectileLauncher = GetComponentInChildren<ProjectileLauncher>();
		projectileLauncher.Initialize(ProjectileLauncherFactory.GetLauncherTypeDefinition(launcherType), Vector2.up);

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
		var projectile = collider.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			ChangeHealth(-projectile.Damage);
			projectile.OnHitSomething();
		}
	}

	private void ChangeHealth(int diff)
	{
		SetHealth(Health + diff);
	}

	private void SetHealth(int health)
	{
		Health = health;
		if (OnHealthChanged != null)
		{
			OnHealthChanged(Health);
		}

		if (Health <= 0)
		{
			OnDied();
		}
	}

	private void SetLives(int lives)
	{
		Lives = lives;
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
		}
	}

	private void OnDied()
	{
		SetLives(Lives - 1);
		SetHealth(maxHealth);
	}

	public delegate void HealthChanged(int newHealth);
	public HealthChanged OnHealthChanged;

	public delegate void LivesChanged(int newLives);
	public LivesChanged OnLivesChanged;

	public delegate void PlayerDied();
	public PlayerDied OnPlayerDead;
}
