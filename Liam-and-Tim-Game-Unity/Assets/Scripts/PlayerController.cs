using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float m_MoveSpeed;
    public float m_PickUpRadius;
    private GameObject m_Weapon;

    // Use this for initialization
    void Start() {
        m_Weapon = null;
    }

    // handle input
    void Update() {
        // turn the character to face whatever direction the mouse is pointed
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mp - (Vector2)transform.position).normalized;
        transform.up = dir;

        // Handle weapon firing
        if (Input.GetKeyUp("space")) {
            if (m_Weapon)
                m_Weapon.GetComponent<GunController>().Shoot();
        }
        // Handle item pickup
        else if (Input.GetKeyUp("f")) {
            // If already carrying a gun, drop it
            if (m_Weapon) {
                m_Weapon.transform.SetParent(transform.parent);
                m_Weapon = null;
            } else {
                // Loop through all guns, and see which are within the pick up radius. Record
                // which gun is closest to the player
                GameObject closestGun = null;
                float closestDist = Mathf.Infinity;
                GameObject[] objects = GameObject.FindGameObjectsWithTag("Weapon");
                foreach (var gun in objects) {
                    Vector3 gun_pos = gun.transform.position;
                    Vector3 player_pos = this.transform.position;
                    float dist = (gun_pos - player_pos).magnitude;
                    if (dist < m_PickUpRadius && dist < closestDist) {
                        closestGun = gun;
                        closestDist = dist;
                    }
                }
                // Now actually pick up the gun
                if (closestGun) {
                    m_Weapon = closestGun;
                    m_Weapon.transform.parent = transform;
                    m_Weapon.transform.up = transform.up;
                    m_Weapon.transform.localPosition = new Vector3(
                        .5f * (transform.localScale.x + m_Weapon.transform.localScale.x),
                        .5f * transform.localScale.y,
                        0);

                }
            }
        }
    }
    void FixedUpdate() {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        // transform.position += move * m_MoveSpeed;
        this.GetComponent<Rigidbody2D>().velocity = move * m_MoveSpeed;
    }
}
