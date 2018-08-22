using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
	public EnemyTypeDefinition TypeDefinition
	{
		get;
		private set;
	}

	public int Health { get; set; }

	public ProjectileLauncherTypeDefinition LauncherTypeDefinition
	{
		get;
		private set;
	}
	private ProjectileLauncher myLauncher;

	private float shotTimer = 0.0f;
	private bool canMoveAndShoot = true;

	public void Initialize(EnemyTypeDefinition typeDef, Vector2 pos)
	{
		TypeDefinition = typeDef;
		gameObject.transform.position = pos;

		gameObject.GetComponent<SpriteRenderer>().sprite = TypeDefinition.Sprite;

		Health = TypeDefinition.StartingHealth;

		LauncherTypeDefinition = ProjectileLauncherFactory.GetLauncherTypeDefinition(typeDef.LauncherType);
		myLauncher = gameObject.GetComponentInChildren<ProjectileLauncher>();
		myLauncher.Initialize(LauncherTypeDefinition, Vector2.down);

		canMoveAndShoot = false;
	}

	// Update is called once per frame
	void Update()
	{
		shotTimer -= Time.deltaTime;

		if (shotTimer < 0.0f)
		{
			ResetShotTimer();
			if (canMoveAndShoot)
			{
				myLauncher.OnFire();
			}
		}
	}

	private void ResetShotTimer()
	{
		//to make more entertaining behavior, each enemy is going to attempt to shoot at random intervals.
		//whether or not the projectile can actually be fired is dependent on the Launcher
		shotTimer = Random.Range(0.1f, 2.0f);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		var projectile = col.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			Health = Health - projectile.Damage;
			if (Health <= 0) Die();
			projectile.OnHitSomething();
		}	
	}

	private void Die()
	{
		GameStats.GlobalStats.ChangeScore(TypeDefinition.PointsForKilling);
		PowerupSystem.Instance.EnemyDied(transform.position);
		gameObject.SetActive(false);
	}

	public void StopMoveAndShoot()
	{
		canMoveAndShoot = false;
	}

	public void StartMoveAndShoot()
	{
		canMoveAndShoot = true;
	}
}
