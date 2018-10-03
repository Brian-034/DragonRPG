using UnityEngine;
using System.Collections;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]

    public class EnemyAI : MonoBehaviour
    {
    //todo remove
        [SerializeField] float chaseRadius = 2f;
        [SerializeField] WayPointContainer patrolPath;
        [SerializeField] float WaypointTolerance = 2f;
 
        PlayerMovement player = null;
        Character character;
        int nextWaypointIndex;
        float currentWeaponRange = 5f;
        float distanceToPlayer;

        enum State { idle,patrolling,attacking,chasing}
        State state = State.idle;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerMovement>();           
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position,player.transform.position );
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if (distanceToPlayer > chaseRadius && state != State.patrolling && patrolPath != null)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if(distanceToPlayer <=currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                // start atacking
                state = State.attacking;
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (true)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(0.5f);
            }
          
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos ) <= WaypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
           
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while(distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

         void OnDrawGizmos()
        {
            //Draw attack sphere
            Gizmos.color = new Color(255f, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            //Draw move sphere
            Gizmos.color = new Color(0, 0, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

        }
    }
}