using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbility
    {

        SelfHealConfig config;


        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
              print("Self Heal behaviour Attaced to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            IncreaseHealth(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            var prefab = Instantiate(config.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticalSystem = prefab.GetComponent<ParticleSystem>();
            myParticalSystem.Play();
            Destroy(prefab, myParticalSystem.main.duration);
        }

        private void IncreaseHealth(AbilityUseParams useParams)
        {
            var player = GetComponent<Player>();
            float currentHealth = player.UpdateCurrentHealth + config.GetExtraHealth();
           // player.UpdateCurrentHealth = currentHealth;

            player.UpdateHealth(-config.GetExtraHealth());
            print("Self Heal Increase Health " + currentHealth);
        }
    }
}

