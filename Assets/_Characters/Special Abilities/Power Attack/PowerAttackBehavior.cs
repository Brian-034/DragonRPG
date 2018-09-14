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
            DealDamage(useParams);
            PlayParticalEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float totalDamage = config.GetExtraDamage() + useParams.baseDamage;
            useParams.target.UpdateHealth(totalDamage);
        }

        private void PlayParticalEffect()
        {
            var prefab = Instantiate(config.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            myParticalSystem.Play();
            Destroy(prefab, myParticalSystem.main.duration);
        }

    }

}

