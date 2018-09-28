using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    public abstract class AbilityBehavior : MonoBehaviour
    {
        protected AbilityConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 5f;

        public abstract void Use(GameObject target);

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticalEffect()
        {
            var particalePrefab = config.GetParticalPrefab();
            var particleObject = Instantiate(particalePrefab, transform.position, particalePrefab.transform.rotation);
            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();            
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}