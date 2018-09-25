using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {         
        public override void Use(AbilityUseParams useParams)
        {
            IncreaseHealth(useParams);
            PlayParticalEffect();
            PlayAbilitySound();
        }
     
        private void IncreaseHealth(AbilityUseParams useParams)
        {
            var player = GetComponent<Player>();
            player.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}

