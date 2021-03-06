﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBehavior
    {
       
        public override void Use(GameObject targets)
        {
            DealRadialDamage();
            PlayParticalEffect();
            PlayAbilitySound();
            PlayAbilityAnimation();
        }

             private void DealRadialDamage()
        {
             // Static sphere cast for targets
               RaycastHit[] hits = Physics.SphereCastAll(
               transform.position,
               (config as AreaEffectConfig).GetRadius(),
               Vector3.up,
               (config as AreaEffectConfig).GetRadius()
               );
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerControl>();
                if (damageable != null && !hitPlayer)
                {
                    float totalDamage = (config as AreaEffectConfig).GetDamageToEachTarget();
                    damageable.TakeDamage(totalDamage);
                }
            }
        }

  
    }
}

