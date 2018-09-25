using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {
           
        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayAbilitySound();            
            PlayParticalEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float totalDamage = (config as PowerAttackConfig).GetExtraDamage() + useParams.baseDamage;
            useParams.target.takeDamage(totalDamage);
        }
    }

}

