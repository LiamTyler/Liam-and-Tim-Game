using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : GunController {

	bool m_triggerDown;

	public SemiAutoGunController () {
		m_triggerDown = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (m_triggerDown) {
			// shoot
		}
	}

	public override void StartAttack () {
		m_triggerDown = true;
	}

	public override void EndAttack () {
		m_triggerDown = false;
	}

	public override void Reload () {
	}

	public override void CycleAction () {
	}
}
