using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility
    {

        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
             print("Area Effect behaviou Attaced to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Area Effect by" + gameObject.name);
            // Static sphere cast for targets
             RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius()
                );
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float totalDamage = config.GetDamageToEachTarget() + useParams.baseDamage;
                    damageable.TakeDamage(totalDamage);
                }
            }


            
        }
    }
}

