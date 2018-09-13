using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
   
    public class Energy : MonoBehaviour
    {
        [SerializeField] Image energyOrb;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 10;
     
        float currentEnergyPoints;

        CameraRaycaster cameraRaycaster;

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            UpdateEnergyBar();

           // cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                RestoreEnergy();
                UpdateEnergyBar();
            }
        }

        private void RestoreEnergy()
        {
            var pointToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointToAdd, 0f, maxEnergyPoints);
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
                 float newEnergyPoints = currentEnergyPoints - amount;
                currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
                UpdateEnergyBar();

        }
     
        void UpdateEnergyBar()
        {
             energyOrb.fillAmount= EnergyAsPercent();
        }

        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}