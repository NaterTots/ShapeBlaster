using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
	public ProjectileTypeDefinition TypeDefinition
	{
		get;
		private set;
	}

	public int Damage { get; private set; }

	public void Initialize(ProjectileTypeDefinition typeDef, Vector2 startingPoint, Vector2 direction)
	{
		TypeDefinition = typeDef;
		gameObject.transform.position = startingPoint;
		GetComponent<Rigidbody2D>().AddForce(direction * typeDef.Speed, ForceMode2D.Impulse);

		gameObject.GetComponent<SpriteRenderer>().sprite = TypeDefinition.Sprite;

		Damage = TypeDefinition.Damage;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnHitSomething()
	{
		//do this here instead of in the enemy/player is in case the projectile should be able to keep going after it hits something
		gameObject.SetActive(false);
	}
}
