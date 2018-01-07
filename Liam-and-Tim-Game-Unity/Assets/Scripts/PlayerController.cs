using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController:MonoBehaviour {
  public float m_MoveSpeed;
  public float m_PickUpRadius;
  private GameObject m_Weapon;
  private iGunController m_gunController;

  // Use this for initialization
  void Start() {
    m_Weapon = null;
    m_gunController = null;
  }

  // handle input
  void Update() {
    // turn the character / gun to face whatever direction the mouse is pointed
    Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 pp = transform.position;
    Vector2 playerDir = mp - pp;
    // TODO(Both): Figure out better gun pointing
    if(m_Weapon) {
      float radius = (pp - (Vector2)(m_gunController
        .GetProjectileSpawnPoint().transform.position)).magnitude;
      Vector2 gunDir = mp - (Vector2)m_Weapon.transform.position;
      if (playerDir.magnitude > radius) {
        transform.up = gunDir.normalized;
      }
      else {
      }
    }
    else {
      transform.up = playerDir.normalized;
    }

    // Handle weapon firing
    if(m_Weapon) {
      if(Input.GetMouseButtonDown(0)) {
        m_gunController.StartAttack();
      }
      if(Input.GetMouseButtonUp(0)) {
        m_gunController.EndAttack();
      }

      if(Input.GetKeyDown("r")) {
        if(m_gunController.GetProjectilesInGun() < m_gunController.GetGunCapacity()) {
          m_gunController.Reload();
        }
      }
    }

    // Handle item pickup
    if(Input.GetKeyDown("f")) {
      // If already carrying a gun, drop it
      if(m_Weapon) {
        m_Weapon.transform.SetParent(transform.parent);
        m_Weapon = null;
        m_gunController = null;
      }
      else {
        // Loop through all guns, and see which are within the pick up radius. Record
        // which gun is closest to the player
        GameObject closestGun = null;
        float closestDist = Mathf.Infinity;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Weapon");
        foreach(var gun in objects) {
          Vector3 gun_pos = gun.transform.position;
          Vector3 player_pos = this.transform.position;
          float dist = (gun_pos - player_pos).magnitude;
          if(dist < m_PickUpRadius && dist < closestDist) {
            closestGun = gun;
            closestDist = dist;
          }
        }
        // Now actually pick up the gun
        if(closestGun) {
          m_Weapon = closestGun;
          m_Weapon.transform.parent = transform;
          m_Weapon.transform.up = transform.up;
          m_Weapon.transform.localPosition = new Vector3(
            0.5f * (transform.localScale.x + m_Weapon.transform.localScale.x),
            0.5f * transform.localScale.y,
            0
          );

          m_gunController = m_Weapon.GetComponent<iGunController>();
        }
      }
    }
  }
  void FixedUpdate() {
    Vector3 move = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
    // transform.position += move * m_MoveSpeed;
    this.GetComponent<Rigidbody2D>().velocity = move * m_MoveSpeed;
  }
}
