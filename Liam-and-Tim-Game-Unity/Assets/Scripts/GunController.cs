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

	public virtual void StartAttack () {
	}

	public virtual void EndAttack () {
	}

    public virtual void Shoot() {
    }

	public virtual void Reload () {
	}

	public virtual void CycleAction () {
	}
}
