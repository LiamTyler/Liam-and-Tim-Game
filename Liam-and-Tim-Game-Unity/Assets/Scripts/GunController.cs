using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : iWeaponController {

	public float m_reloadTime;
	public float m_actionTime;
	public int m_projectilesPerReload;
	public float m_projectileStartV;

	virtual void StartAttack () {
	}

	virtual void EndAttack () {
	}

	virtual void Reload () {
	}

	virtual void CycleAction () {
	}
}
