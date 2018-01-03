using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController:MonoBehaviour, iWeaponController {
  public float m_reloadTime;  // How long a reload takes
  public float m_actionTime;  // How long a cycle action takes
  protected float m_NextFireTime;  // Time when cycle action will be done
  protected float m_ReloadCompleteTime;  // Time when reloading will be done
  public GameObject m_Projectile;  // The bullet prefab that is being fired
  public Transform m_ProjectileSpawnPoint;  // Location of where the bullet is firing from
  public int m_projectilesPerReload;
  public float m_projectileStartV;  // Bullet's starting speed
  public float m_ProjectileLifeTime;  // How long the bullet will last, given no collisions

  protected int m_projectilesInGun;  // Clip capacity
  protected bool m_Reloading;  // True if currently reloading
  protected bool m_triggerDown;  // True if the fire button is down

  public virtual void Start() {
    m_projectilesInGun = 0;
    m_Reloading = false;
    m_triggerDown = false;
    m_NextFireTime = 0;
    m_ReloadCompleteTime = Mathf.Infinity;
  }

  public virtual void StartAttack() {
    m_triggerDown = true;
  }

  public virtual void EndAttack() {
    m_triggerDown = false;
  }

  public virtual void Update() {
    if (m_Reloading && Time.time >= m_ReloadCompleteTime) {
      m_Reloading = false;
      m_projectilesInGun = m_projectilesPerReload;
    } else if(m_triggerDown && !m_Reloading && Time.time > m_NextFireTime) {
      Shoot();
    }
  }

  public virtual void Shoot() {
    if(m_projectilesInGun > 0) {
      GameObject newBullet = Instantiate(
        m_Projectile,
        m_ProjectileSpawnPoint.transform.position,
        Quaternion.LookRotation(Vector3.forward,transform.up));
      newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * m_projectileStartV;
      Destroy(newBullet,m_ProjectileLifeTime);
      --m_projectilesInGun;

      CycleAction();
    }
  }

  public virtual void Reload() {
    m_ReloadCompleteTime = Time.time + m_reloadTime;
    m_Reloading = true;
  }

  public virtual void CycleAction() {
    m_NextFireTime = Time.time + m_actionTime;
  }
}
