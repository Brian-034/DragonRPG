using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]

    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField]   float Radius = 5f;
        [SerializeField]   float damageToEachTarget = 15f;

        public override void AttachComponentTo(GameObject gameObjectToAttachhTo)
        {
            var behviourComponent = gameObjectToAttachhTo.AddComponent<AreaEffectBehavior>();
            behviourComponent.SetConfig(this);
            behaviour = behviourComponent;
        }

       public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return Radius;
        }
    }
}
