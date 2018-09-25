﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBehavior
    {
       
        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticalEffect();
            PlayAbilitySound();
        }

             private void DealRadialDamage(AbilityUseParams useParams)
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
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float totalDamage = (config as AreaEffectConfig).GetDamageToEachTarget() + useParams.baseDamage;
                    damageable.takeDamage(totalDamage);
                }
            }
        }

  
    }
}

