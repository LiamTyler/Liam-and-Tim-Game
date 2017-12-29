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
		
	}

/*	public virtual void StartAttack () {
		m_triggerDown = true;
	}

	public virtual void EndAttack () {
		m_triggerDown = false;
	}

	public virtual void Reload () {
	}

	public virtual void CycleAction () {
	}*/
}
