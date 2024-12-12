using UnityEngine;

namespace KeySystem
{
    public class KeyRaycast : MonoBehaviour
    {

        [Header("Raycast Settings")]
        [SerializeField]
        private float _RayDistance = 5f;

        [SerializeField]
        private LayerMask _LayerMaskInteract;

        [SerializeField]
        private Transform _PlayerCamera;

        [SerializeField]
        private string _InteractebleTag = "InteractiveObject";

        public GameObject KeyHintHUD;

        private KeyItemController _KeyRaycastObject;
        private FuseController _FuseRaycastObject;

        private void Start()
        {
            Debug.Log("I'm in " + this.gameObject.name);
            if (KeyHintHUD != null)
                KeyHintHUD.SetActive(false);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _PlayerCamera = player.GetComponentInChildren<Camera>().transform;
                if (player.TryGetComponent<ChangeView>(out var changeView))
                {
                    changeView.OnCameraChanged += SetCamera;
                }
            }
        }

        private void SetCamera()
        {
            Debug.Log("SetCamera");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _PlayerCamera = player.GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            if (_PlayerCamera == null)
            {
                Debug.LogWarning("Player Camera is not assigned.");
                return;
            }

            Ray ray = new Ray(_PlayerCamera.position, _PlayerCamera.forward);

            //Debug.DrawRay(_PlayerCamera.position, _PlayerCamera.forward * _RayDistance, Color.green);

            if (Physics.Raycast(ray, out RaycastHit hit, _RayDistance, _LayerMaskInteract))
            {
                Debug.Log("Hit: " + hit.transform.gameObject.name);

                Debug.DrawRay(_PlayerCamera.position, _PlayerCamera.forward * _RayDistance, Color.red);

                if (hit.transform.CompareTag(_InteractebleTag))
                {
                    if (KeyHintHUD != null)
                        KeyHintHUD.SetActive(true);

                    _KeyRaycastObject = hit.transform.GetComponent<KeyItemController>();
                    _FuseRaycastObject = hit.transform.GetComponent<FuseController>();

                    if (Input.GetKeyDown(KeyCode.E))
                    { // TODO: Change to InputManager
                        if (_KeyRaycastObject != null)
                            _KeyRaycastObject.ObjectInteraction();

                        if (_FuseRaycastObject != null)
                            _FuseRaycastObject.ObjectInteraction();
                    }
                }
                else if (KeyHintHUD != null)
                {
                    KeyHintHUD.SetActive(false);
                }
            }
            else
            {
                if (KeyHintHUD != null)
                    KeyHintHUD.SetActive(false);

                _KeyRaycastObject = null;
                _FuseRaycastObject = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_PlayerCamera != null)
            {
                Gizmos.color = Color.red;
                Vector3 direction = _PlayerCamera.forward * _RayDistance;
                Gizmos.DrawRay(_PlayerCamera.position, direction);
            }
        }
    }
}
