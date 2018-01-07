using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iGunController {

  void StartAttack();
  void EndAttack();
  void Reload();
  void CycleAction(bool manual);
  bool GunCocked();
  int GetProjectilesInGun();
  int GetGunCapacity();
  Transform GetProjectileSpawnPoint();
}
