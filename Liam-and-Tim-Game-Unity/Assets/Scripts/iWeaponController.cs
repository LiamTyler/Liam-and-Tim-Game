using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iWeaponController {

	void StartAttack ();
	void EndAttack ();
	void Reload ();
	void CycleAction ();
}
