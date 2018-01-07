using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : AutoGunController {
  public bool m_triggerReset; // semi auto guns must reset trigger mechanism to fire again

  public override void Start() {
    base.Start();
    m_triggerReset = true;
  }

  public override void StartAttack() {
    m_triggerDown = true;
  }

  public override void EndAttack() {
    base.EndAttack();
    m_triggerReset = true;
  }

  protected override void Shoot() {
    base.Shoot();
    m_triggerReset = false;
  }

  protected override bool ReadyToShoot() {
    return m_triggerReset && base.ReadyToShoot();
  }
}
