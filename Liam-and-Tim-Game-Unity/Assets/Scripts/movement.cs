using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public GameObject Bullet;
	public float moveSpeed = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;

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
