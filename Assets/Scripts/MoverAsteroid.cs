using System.Collections;
using UnityEngine;

public class MoverAsteroid : MonoBehaviour
{
	private float speed;


	void Start()
	{
		speed = -5 * (Random.Range(1,4));
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

}