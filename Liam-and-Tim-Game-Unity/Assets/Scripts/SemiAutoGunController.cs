using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : MonoBehaviour, GunController {

	bool m_triggerDown;

	public SemiAutoGunController () {
		triggerDown = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		
	}

	virtual void StartAttack () {
		triggerDown = true;
	}

	virtual void EndAttack () {
		triggerDown = false;
	}

	virtual void Reload () {
	}

	virtual void CycleAction () {
	}
}
