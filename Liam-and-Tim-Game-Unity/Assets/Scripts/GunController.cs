using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour, iWeaponController {

	public float m_reloadTime;
	public float m_actionTime;
	public GameObject m_Projectile;
	public Transform m_ProjectileSpawnPoint;
	public int m_projectilesPerReload;
	public float m_projectileStartV;
	public float m_ProjectileLifeTime;

	protected int m_projectilesInGun;
	protected bool m_readyToFire;
	protected bool m_triggerDown;

	public virtual void StartAttack () {
		m_triggerDown = true;
	}

	public virtual void EndAttack () {
		m_triggerDown = false;
	}

	public virtual void Shoot() {
		m_readyToFire = false;
      // Spawn bullet and give it a velocity. (bullet only lasts 2 seconds right now)
		if(m_projectilesInGun > 0)
		{
	      GameObject newBullet = Instantiate(
	      m_Projectile,
	      m_ProjectileSpawnPoint.transform.position,
	      Quaternion.LookRotation(Vector3.forward, transform.up));
	      newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * m_projectileStartV;
	      Destroy(newBullet, m_ProjectileLifeTime);
			--m_projectilesInGun;

	      CycleAction ();
		}
	}

	public virtual void Reload () {
		// add delay based on reload time
		m_projectilesInGun = m_projectilesPerReload;
	}

	public virtual void CycleAction () {
		// some delay to limit fire rate
		if(!m_triggerDown && m_projectilesInGun > 0)
		{
			m_readyToFire = true;
		}
		print("action cycled"); // debug
	}
}
