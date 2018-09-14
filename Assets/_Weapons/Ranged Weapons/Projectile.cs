﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = 10f;
        [SerializeField] GameObject shooter; // Only so can inspect when paused

        const float DESTROY_DELAY = 0.01f;

        float damageCaused;
        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }
   

        void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                DamageDamageblObjects(collision);
            }
        }

        private void DamageDamageblObjects(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).UpdateHealth(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}
