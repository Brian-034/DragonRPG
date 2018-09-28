using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class Player : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10f;
     
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
         [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

         [SerializeField] ParticleSystem criticalHiPartical = null;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        
        Enemy currentEnemy = null;

        Animator animator = null;
        SpecialAbilities abilities;

        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0f;
        SpecialAbilities energy = null;
        GameObject weaponObject;


        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();
            RegisterForMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();        
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }
 
        public void Update()
        {
            var healthPercent = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercent > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAblity(keyIndex);
                }
            }           
        }
     
        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();

         }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand Found");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple Dominant Hands found");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
         }

       void OnMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;
            if (Input.GetMouseButton(0) && IsTargetInRamge(enemy.gameObject))
            {
                AttackTarget();
            }
           else if (Input.GetMouseButtonDown(1) && IsTargetInRamge(enemy.gameObject)) //bjc
            {
                abilities.AttemptSpecialAblity(0, enemy.gameObject);
             }

        }

        private bool IsTargetInRamge(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
           
        }

        private void AttackTarget()
        {          
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                currentEnemy.takeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            float totalDamage = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            bool isCriticalHit = UnityEngine.Random.Range(0, 1f) <= criticalHitChance;
            if (isCriticalHit)
            {
                totalDamage *= criticalHitMultiplier;
                criticalHiPartical.Play();
            }
             return totalDamage;
        }
  
      }
}
