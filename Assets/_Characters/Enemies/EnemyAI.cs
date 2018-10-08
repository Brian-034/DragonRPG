using UnityEngine;
using System.Collections;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]

    public class EnemyAI : MonoBehaviour
    {
    //todo remove
        [SerializeField] float chaseRadius = 2f;
        [SerializeField] WayPointContainer patrolPath;
        [SerializeField] float WaypointTolerance = 2f;
        [SerializeField] float WaypointDwellTime = 2f;

        PlayerControl player = null;
        Character character;
        int nextWaypointIndex;
        float distanceToPlayer;
        float currentWeaponRange =2f;

        enum State { idle,patrolling,attacking,chasing}
        State state = State.idle;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();           
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position,player.transform.position );
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseRing = distanceToPlayer > currentWeaponRange 
                                &&  distanceToPlayer <= chaseRadius;
            bool outsideChaseRadius = distanceToPlayer > chaseRadius;

            if (outsideChaseRadius)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                if (patrolPath != null)
                {
                    StartCoroutine(Patrol());
                }            
            }
            if (inChaseRing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
                weaponSystem.StopAttacking();
            }
            if(inWeaponCircle)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(WaypointDwellTime);
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