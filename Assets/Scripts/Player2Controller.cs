﻿using System.Collections;
using UnityEngine;

[System.Serializable]
public class Boundary2
{
	public float xMin, xMax, zMin, zMax;
}

public class Player2Controller : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary2 boundary;
	private DestroyByContact destroyByContact;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private float nextFire;

	void Update()
	{
		if(Input.GetButton("Fire2") && Time.time > nextFire)
		{
			GameObject gameControllerObject = GameObject.FindWithTag("GameController");
			if( gameControllerObject != null )
			{
				GameController gameController = gameControllerObject.GetComponent<GameController>();
				gameController.AddScore(-2);
			}
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		}
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal2");
		float moveVertical = Input.GetAxis("Vertical2");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().position = new Vector3
			(
				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
			);

		GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
	void FireBigShot()
	{
		nextFire = Time.time + fireRate;
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		GetComponent<AudioSource>().Play();
	}
	public void increaseFireRate() {
		fireRate += -0.05f;
	}
}