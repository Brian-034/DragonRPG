
using UnityEngine;
namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particalPrefab;
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] AnimationClip animationClip;

        protected AbilityBehavior behaviour;

        public abstract AbilityBehavior GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject ObjectToAttachhTo)
        {
            AbilityBehavior behaviourComponent = GetBehaviourComponent(ObjectToAttachhTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
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

        public AnimationClip GetAnimationClip()
        {
            return animationClip;
        }
    }

}
