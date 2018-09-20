using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {
        PowerAttackConfig config;
        AudioSource audioSource;

        public void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }
 
        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlaySound();
            PlayParticalEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float totalDamage = config.GetExtraDamage() + useParams.baseDamage;
            useParams.target.takeDamage(totalDamage);
        }

        private void PlayParticalEffect()
        {
            var prefab = Instantiate(config.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            myParticalSystem.Play();
            Destroy(prefab, myParticalSystem.main.duration);
        }

        private void PlaySound()
        {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }


    }

}

