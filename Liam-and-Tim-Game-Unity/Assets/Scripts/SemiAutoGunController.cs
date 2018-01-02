using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : GunController {

    public SemiAutoGunController () {
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
	}

	public override void StartAttack () {
        Shoot();
	}

	public override void EndAttack () {
	}

    public override void Shoot() {
        // Spawn bullet and give it a velocity. (bullet only lasts 2 seconds right now)
        GameObject newBullet = Instantiate(
            m_Projectile,
            m_ProjectileSpawnPoint.transform.position,
            Quaternion.LookRotation(Vector3.forward, transform.up));
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * m_projectileStartV;
        Destroy(newBullet, m_ProjectileLifeTime);
    }

    public override void Reload () {
	}

	public override void CycleAction () {
	}
}
