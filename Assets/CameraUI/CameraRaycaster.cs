using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 9;
        float maxRaycastDepth = 100f; // Hard coded value

        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
   
        // Setup delegates for broadcasting layer changes to other classes
   
        public delegate void OnMouseOverTarrain(Vector3 destination);
        public event OnMouseOverTarrain onMouseOverPotentaillyWalkable;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;


        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Implement UI interaction
            }
            else
            {
                PerformRaycast();
            }           
        }

        private void PerformRaycast()
        {
            if (screenRect.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Specify layer priority
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForWalkable(ray)) { return; }
            }
        }

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        private bool RaycastForWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potetiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool WalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potetiallyWalkableLayer);
            if (WalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentaillyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }

      }
}