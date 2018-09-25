
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particalPrefab;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehavior behaviour;

        public abstract AbilityBehavior GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject ObjectToAttachhTo)
        {
            AbilityBehavior behaviourComponent = GetBehaviourComponent(ObjectToAttachhTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(AbilityUseParams useParams)
        {
            behaviour.Use(useParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }
     
    
        public GameObject GetParticalPrefab()
        {
            return particalPrefab;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0,audioClips.Length)];
        }

    }

}
