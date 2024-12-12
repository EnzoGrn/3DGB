using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem {
    public class KeyRaycast : MonoBehaviour {

        [Header("Raycast Settings")]
        [SerializeField]
        private int _RayDistance = 5;

        [SerializeField]
        private LayerMask _LayerMaskInteract;

        [SerializeField]
        private string _ExcludeLayerName = null;

        private KeyItemController _RaycastObject;

        private string _InteractebleTag = "InteractiveObject";

        private void Start () {
            Debug.Log("I'm in " + this.gameObject.name);
        }

        private void Update()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            int mask = 1 << LayerMask.NameToLayer(_ExcludeLayerName) | _LayerMaskInteract.value;

            if (Physics.Raycast(transform.position, forward, out hit, _RayDistance, mask)) {
                if (hit.collider.CompareTag(_InteractebleTag)) {
                    _RaycastObject = hit.collider.GetComponent<KeyItemController>();

                    if (Input.GetKeyDown(KeyCode.E)) // TODO: Change to InputManager
                        _RaycastObject.ObjectInteraction();
                }
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Vector3 direction = transform.TransformDirection(Vector3.forward) * _RayDistance;
        //    Gizmos.DrawRay(transform.position, direction);
        //}
    }
}