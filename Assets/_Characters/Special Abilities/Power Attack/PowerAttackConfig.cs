﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbility
    {
        [Header("Power Attack Specific")]
        [SerializeField]  float extraDamage = 10f;

        public override void AttachComponentTo(GameObject gameObjectToAttachhTo)
        {
            var behviourComponent = gameObjectToAttachhTo.AddComponent<PowerAttackBehavior>();
            behviourComponent.SetConfig(this);
            behaviour= behviourComponent;
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }

       
    }
}