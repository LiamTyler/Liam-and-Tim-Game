using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGunController : MonoBehaviour, iGunController {
  public float m_reloadTime;  // How long a reload takes
  public float m_actionTime;  // How long auto cycle action takes
  public float m_manualActionTime;  // How long manual cycle action takes
  public GameObject m_projectile;  // The bullet prefab that is being fired
  public Transform m_projectileSpawnPoint;  // Location of where the bullet is firing from
  public Transform m_restingPosition;  // default position of gun (when not reloading, cycling, etc.)
  public int m_gunCapacity;  // how many projectiles the gun can hold at once
  public float m_projectileStartV;  // Bullet's starting speed
  public float m_projectileLifeTime;  // How long the bullet will last, given no collisions
  public int m_projectilesInGun;
  public bool m_gunCocked;
  public bool m_reloading;  // True if currently reloading
  public bool m_cycling;
  public bool m_triggerDown;  // True if the fire button is down


  public virtual void Start() {
    m_projectilesInGun = 0;
    m_reloading = false;
    m_cycling = false;
    m_triggerDown = false;
    m_gunCocked = false;
  }

  public virtual void StartAttack() {
    m_triggerDown = true;
  }

  public virtual void EndAttack() {
    m_triggerDown = false;
  }

  public virtual void Update() {
    if (m_triggerDown && ReadyToShoot()) {
      Shoot();
    }
  }

  public virtual void Reload() {
    // if we're already reloading, don't start reloading again
    if(m_reloading) {
      return;
    }
    m_reloading = true;
    transform.Rotate(0, 0, 20);
    Invoke("ReloadHelper", m_reloadTime);
  }

  public virtual void CycleAction(bool manual) {
    // if we're already cycling action, don't start cycling again
    if(m_cycling) {
      return;
    }

    m_cycling = true;
    transform.Translate(0, -0.1f, 0);
    if (manual) {
      Invoke("CycleHelper", m_manualActionTime);
    }
    else {
      Invoke("CycleHelper", m_actionTime);
    }
  }

  public virtual int GetGunCapacity() {
    return m_gunCapacity;
  }

  public virtual int GetProjectilesInGun() {
    return m_projectilesInGun;
  }

  public virtual bool GunCocked() {
    return m_gunCocked;
  }

  public virtual Transform GetProjectileSpawnPoint() {
    return m_projectileSpawnPoint;
  }

  protected virtual void Shoot() {
    m_gunCocked = false;
    GameObject newBullet = Instantiate(
      m_projectile,
      m_projectileSpawnPoint.transform.position,
      Quaternion.LookRotation(Vector3.forward, transform.up)
    );
    newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * m_projectileStartV;
    Destroy(newBullet, m_projectileLifeTime);

    // auto guns cycle action if a bullet is left in the gun
    if(--m_projectilesInGun > 0) {
      CycleAction(false);
    }

    // prevent bullet from colliding with the gun and disappearing immediately
    Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
  }

  protected virtual bool ReadyToShoot() {
    return m_projectilesInGun > 0 && m_gunCocked && !m_reloading && !m_cycling;
  }

  // this method should be called asynchronously
  // e.g. Invoke("ReloadHelper", delay);
  protected virtual void ReloadHelper() {
    // stuff to do after delay
    transform.Rotate(0, 0, -20);
    if(!m_gunCocked) {
      CycleAction(true);
    }
    m_projectilesInGun = m_gunCapacity;
    m_reloading = false;
  }

  // this method should be called asynchronously
  // e.g. Invoke("CycleHelper", delay);
  protected virtual void CycleHelper() {
    // stuff to do after delay
    transform.Translate(0, 0.1f, 0);
    m_gunCocked = true;
    m_cycling = false;
  }
}
