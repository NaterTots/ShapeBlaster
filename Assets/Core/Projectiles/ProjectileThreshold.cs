using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThreshold : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col)
	{
		col.gameObject.SetActive(false);
	}
}
