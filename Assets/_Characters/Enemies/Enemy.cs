﻿using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float attackRadius = 5f;
        [SerializeField] float chaseRadius = 2f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInSec = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        ThirdPersonCharacter thirdPersonCharacter;
        AICharacterControl aiCharacterControl;
        Player player = null;
        bool isAttacking = false;
        float currentHealthPoints;

        void Start()
        {
            player = FindObjectOfType<Player>();
            thirdPersonCharacter = player.GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = gameObject.GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            if (player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);
            }
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomizedDelay = Random.Range(firingPeriodInSec - firingPeriodVariation, firingPeriodInSec + firingPeriodVariation);
                InvokeRepeating("SpawnProjectiles", 0, randomizedDelay);
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }


            if (distanceToPlayer < chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void takeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }

        void SpawnProjectiles()
        {

            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity) as GameObject;
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos()
        {
            //Draw attack sphere
            Gizmos.color = new Color(255f, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            //Draw move sphere
            Gizmos.color = new Color(0, 0, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

        }
    }
}