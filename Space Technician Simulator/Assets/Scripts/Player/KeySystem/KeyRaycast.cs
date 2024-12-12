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

        private KeyItemController _KeyRaycastObject;
        private FuseController    _FuseRaycastObject;

        private string _InteractebleTag = "InteractiveObject";

        public GameObject KeyHintHUD;

        private void Start () {
            Debug.Log("I'm in " + this.gameObject.name);
            KeyHintHUD.SetActive(false);
        }

        private void Update()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            int mask = 1 << LayerMask.NameToLayer(_ExcludeLayerName) | _LayerMaskInteract.value;

            if (Physics.Raycast(transform.position, forward, out hit, _RayDistance, mask)) {
                if (hit.collider.CompareTag(_InteractebleTag)) {
                    KeyHintHUD.SetActive(true);
                    _KeyRaycastObject  = hit.collider.GetComponent<KeyItemController>();
                    _FuseRaycastObject = hit.collider.GetComponent<FuseController>();

                    if (Input.GetKeyDown(KeyCode.E) && _KeyRaycastObject) // TODO: Change to InputManager
                        _KeyRaycastObject.ObjectInteraction();
                    if (Input.GetKeyDown(KeyCode.E) && _FuseRaycastObject) // TODO: Change to InputManager
                        _FuseRaycastObject.ObjectInteraction();
                }
            } else {
                KeyHintHUD.SetActive(false);
                _KeyRaycastObject = null;
                _FuseRaycastObject = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * _RayDistance;
            Gizmos.DrawRay(transform.position, direction);
        }
    }
}
