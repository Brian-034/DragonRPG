using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [SelectionBase]
  
    public class Character : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] float audioVolume = 0.2f;
        [SerializeField] float audioSpacialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0f, 1.03f, 0f);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.11f;

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar avatar;
      
        [Header("Movement")]
         [SerializeField] float moveSpeedMultiplyer = 1f;
        [SerializeField] float animationSpeedMultiplier = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshSpeed = 1f;
        [SerializeField] float navMeshAngularSpeed = 120f;
        [SerializeField] float navMeshAcceleration = 20f;
        [SerializeField] float navMeshStoppingDistance = 1.3f;

      
        Animator animator;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;
        NavMeshAgent navMeshAgent;
        Rigidbody myRigidbody;
        bool isAlive = true;

        void Awake()
        {            
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var audioScource = gameObject.AddComponent<AudioSource>();
            audioScource.volume = audioVolume;
            audioScource.playOnAwake = false;
            audioScource.loop = false;
            audioScource.spatialBlend = audioSpacialBlend;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;
            capsuleCollider.isTrigger = false;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = avatar;
            animator.applyRootMotion = true;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.stoppingDistance = navMeshStoppingDistance;
            navMeshAgent.speed = navMeshSpeed;
            navMeshAgent.angularSpeed = navMeshAngularSpeed;
            navMeshAgent.autoBraking = false;

        }

        //void Start()
        //{
        //    CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        //    cameraRaycaster.onMouseOverPotentaillyWalkable += MovePlayer;
        //    cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        // }

        void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }

        void MovePlayer(Vector3 Destination)
        {
            if (Input.GetMouseButton(0))
            {
                navMeshAgent.SetDestination(Destination);
            }
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0)  || Input.GetMouseButtonDown(1))
            {
                navMeshAgent.SetDestination(enemy.transform.position);
            }

        }


      void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplyer) / Time.deltaTime;
                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            //movement = Vector3.ProjectOnPlane(movement, groundNormal);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }


        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;


            //// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            //// which affects the movement speed because of the root motion.
            //if ( move.magnitude > 0)
            //{
            //	animator.speed = animSpeedMultiplier;
            //}

        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }


    }
}