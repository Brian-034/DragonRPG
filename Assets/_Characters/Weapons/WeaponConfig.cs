using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class WeaponConfig : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }
        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimatioEvants(); return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        //So that asset packs do not cause crashes
        private void RemoveAnimatioEvants()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
