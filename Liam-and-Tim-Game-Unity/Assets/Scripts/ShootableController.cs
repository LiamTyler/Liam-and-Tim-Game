using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableController : MonoBehaviour {
  public float m_Health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void Die() {
    Destroy(gameObject);
  }

  public void Damage(float baseDamage) {
    m_Health -= baseDamage;
    if (m_Health <= 0) {
      Die();
    }
  }
}
