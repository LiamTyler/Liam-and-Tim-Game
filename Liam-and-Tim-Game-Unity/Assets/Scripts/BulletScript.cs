using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
  public float m_BaseDamage;
  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  float CalculateBaseDamage() {
    return m_BaseDamage;
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Shootable") {
      other.gameObject.GetComponent<ShootableController>().Damage(CalculateBaseDamage());
    }
    Destroy(gameObject);
  }
}
