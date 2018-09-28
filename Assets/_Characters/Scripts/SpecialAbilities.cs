using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{  
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 10;
        [SerializeField] AudioClip OutOfEnergy;

        float currentEnergyPoints;
        AudioSource audioSource;


        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }
    
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;           
            UpdateEnergyBar();
            AttachInitialAbilities();
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                RestoreEnergy();
                UpdateEnergyBar();
            }
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttemptSpecialAblity(int abilityIndex, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);

                //    var abilityParams = new AbilityUseParams(currentEnemy, baseDamage);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(OutOfEnergy);
                }
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        void RestoreEnergy()
        {
            var pointToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointToAdd, 0f, maxEnergyPoints);
        }

        public void ConsumeEnergy(float amount)
        {
                 float newEnergyPoints = currentEnergyPoints - amount;
                currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
                UpdateEnergyBar();

        }
     
        void UpdateEnergyBar()
        {
            energyBar.fillAmount= energyAsPercent;
        }
   }
}