using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoGunController : GunController {

   public SemiAutoGunController () {
   }

   // Use this for initialization
   void Start () {
      m_projectilesInGun = 0;
      m_readyToFire = false;
      m_triggerDown = false;
   }

   // Update is called once per frame
   void Update () {

   }

   void FixedUpdate() {
      if(m_triggerDown && m_readyToFire)
      {
         Shoot();
      }
   }

   //public override void StartAttack () {
   //}

   public override void EndAttack () {
      base.EndAttack();
      if(0 != m_projectilesInGun)
      {
         m_readyToFire = true;
      }
   }

   //public override void Shoot() {
   //}

   //public override void Reload () {
   //}

   //public override void CycleAction () {
   //}
}
