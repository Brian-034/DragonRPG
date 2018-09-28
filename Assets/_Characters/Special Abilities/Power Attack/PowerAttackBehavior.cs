using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {
           
        public override void Use(GameObject target)
        {
            DealDamage(target);
            PlayAbilitySound();            
            PlayParticalEffect();
        }

        private void DealDamage(GameObject target)
        {
            float totalDamage = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(totalDamage);
        }
    }

}

