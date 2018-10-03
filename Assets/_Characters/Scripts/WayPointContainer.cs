using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WayPointContainer : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;
             Gizmos.color = new Color(255f, 0, 0, 0.7f);
 
            foreach(Transform waypoint in transform)
            {
                 Gizmos.DrawSphere(waypoint.position, 0.2f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, firstPosition);
        }
    }
}