using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
           // print("Power attack behaviou on " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack by" + gameObject.name);
            float totalDamage = config.GetExtraDamage() + useParams.baseDamage;
            useParams.target.TakeDamage(totalDamage);
        }
    }

}

