using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility
    {

        AreaEffectConfig config;
       

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
           //  print("Area Effect behaviou Attaced to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            var prefab = Instantiate(config.GetParticalPrefab(),transform.position,Quaternion.identity);
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            myParticalSystem.Play();
            Destroy(prefab, myParticalSystem.main.duration);
         }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
             // Static sphere cast for targets
               RaycastHit[] hits = Physics.SphereCastAll(
               transform.position,
               config.GetRadius(),
               Vector3.up,
               config.GetRadius()
               );
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float totalDamage = config.GetDamageToEachTarget() + useParams.baseDamage;
                    damageable.UpdateHealth(totalDamage);
                }
            }
        }
    }
}

