using UnityEngine;
using System.Collections;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]

    public class EnemyAI : MonoBehaviour
    {
    //todo remove
        [SerializeField] float chaseRadius = 2f;
        //[SerializeField] float damagePerShot = 9f;
        //[SerializeField] float firingPeriodInSec = 0.5f;
        //[SerializeField] float firingPeriodVariation = 0.1f;
 
        PlayerMovement player = null;
        Character character;
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
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                // start patrolling
                state = State.patrolling;
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