using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerParent {

	public GameObject Bullet;

	// Use this for initialization
	void Start () {
		
	}

	// handle input
	void Update() {
		if (Input.GetKeyUp("space"))
		{
			//  GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
			GameObject bullet = Instantiate(
				Bullet,
				transform.position,
				Quaternion.identity);

			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * 4;

			Destroy(bullet, 2.0f);
		}
	}	
}
