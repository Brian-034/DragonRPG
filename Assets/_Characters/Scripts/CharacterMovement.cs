using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]

    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplyer = 1f;
        [SerializeField] float animationSpeedMultiplier = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        Animator animator;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;
        NavMeshAgent agent;
        Rigidbody myRigidbody;

        bool isInDirectMode = false;

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true;
            agent.updateRotation = false;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentaillyWalkable += MovePlayer;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

            animator = GetComponent<Animator>();
            animator.applyRootMotion = true;

            myRigidbody = GetComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
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

        public void Kill()
        {

        }

        void MovePlayer(Vector3 Destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(Destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0)  || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
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