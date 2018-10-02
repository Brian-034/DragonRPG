using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {

         [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

         [SerializeField] ParticleSystem criticalHiPartical = null;

      
          
        SpecialAbilities abilities;

        CameraRaycaster cameraRaycaster = null;
        
        SpecialAbilities energy = null;

        Character character;
        WeaponSystem weaponSystem;

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();
         }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentaillyWalkable += onMouseOverPotentiallyWalkable;
        }

       public void Update()
       {
                ScanForAbilityKeyDown();
       }

 
        void onMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
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
     
        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRamge(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
           else if (Input.GetMouseButtonDown(1) && IsTargetInRamge(enemy.gameObject)) //bjc
            {
                abilities.AttemptSpecialAblity(0, enemy.gameObject);
             }

        }

        private bool IsTargetInRamge(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
           
        }

      }
}
