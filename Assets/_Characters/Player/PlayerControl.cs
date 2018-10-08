using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {     
        SpecialAbilities abilities;

       
        
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
            CameraRaycaster cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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
                weaponSystem.StopAttacking();
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
            else if (Input.GetMouseButton(0) && !IsTargetInRamge(enemy.gameObject))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
           else if (Input.GetMouseButtonDown(1) && IsTargetInRamge(enemy.gameObject)) //bjc
           {
                abilities.AttemptSpecialAblity(0, enemy.gameObject);
           }
           else if (Input.GetMouseButtonDown(1) && !IsTargetInRamge(enemy.gameObject)) //bjc
           {
                StartCoroutine(MoveAndPowerAttack(enemy));
           }

        }

        private bool IsTargetInRamge(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
           
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while (!IsTargetInRamge(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAblity(0, enemy.gameObject);
        }
    }
}
