using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Powerup : MonoBehaviour
{
	public PowerupTypeDefinition TypeDefinition
	{
		get;
		private set;
	}

	public void Initialize(PowerupTypeDefinition typeDef, Vector2 position)
	{
		TypeDefinition = typeDef;
		gameObject.transform.position = position;
		GetComponent<Rigidbody2D>().AddForce(Vector2.down * 2.0f, ForceMode2D.Impulse);
		GetComponent<Rigidbody2D>().AddTorque(3.0f, ForceMode2D.Impulse);

		gameObject.GetComponent<SpriteRenderer>().sprite = TypeDefinition.Sprite;
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnHitSomething()
	{
		gameObject.SetActive(false);
	}
}
