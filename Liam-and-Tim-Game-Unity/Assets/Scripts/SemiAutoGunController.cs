using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : GunController {
  protected float m_LastCycleActionTime;

  public override void EndAttack() {
    base.EndAttack();
    m_NextFireTime = m_LastCycleActionTime + m_actionTime;
  }

  public override void CycleAction() {
    m_LastCycleActionTime = Time.time;
    if (m_triggerDown)
      m_NextFireTime = Mathf.Infinity;
  }
}
