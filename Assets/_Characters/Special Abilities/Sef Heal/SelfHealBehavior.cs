using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbility
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
       
        public void Use(AbilityUseParams useParams)
        {
            IncreaseHealth(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            var prefab = Instantiate(config.GetParticalPrefab(), transform.position, Quaternion.identity);
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

