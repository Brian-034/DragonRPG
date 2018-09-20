using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {

        SelfHealConfig config;
        AudioSource audioSource;

        public void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }
       
        public override void Use(AbilityUseParams useParams)
        {
            IncreaseHealth(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            var particalePrefab = config.GetParticalPrefab();
            var prefab = Instantiate(particalePrefab, transform.position, particalePrefab.transform.rotation);
            prefab.transform.parent = transform;
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            myParticalSystem.Play();
            Destroy(prefab, myParticalSystem.main.duration);
        }

        private void IncreaseHealth(AbilityUseParams useParams)
        {
            PlaySound();
            var player = GetComponent<Player>();
            player.Heal(config.GetExtraHealth());
        }

        private void PlaySound()
        {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }
    }
}

