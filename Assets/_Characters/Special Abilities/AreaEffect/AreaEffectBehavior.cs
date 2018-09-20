using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBehavior
    {

        AreaEffectConfig config;
        AudioSource audioSource;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
             audioSource = gameObject.GetComponent<AudioSource>();
             audioSource.playOnAwake = false;
        }

        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            var particalePrefab = config.GetParticalPrefab();
            var prefab = Instantiate(particalePrefab, transform.position, particalePrefab.transform.rotation);
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            PlaySound();
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
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float totalDamage = config.GetDamageToEachTarget() + useParams.baseDamage;
                    damageable.takeDamage(totalDamage);
                }
            }
        }

        private void PlaySound()
        {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

    }
}

