using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Scanner : MonoBehaviour {

    [Header("Events to trigger")]
    public UnityEvent<bool> DoAction;
    public UnityEvent<bool> UndoAction;

    [Header("Value")]
    [SerializeField]
    private bool _IsScanned = false;

    [SerializeField]
    private bool _IsPlayerNearby = false;

    [SerializeField]
    private Transform _PlayerCamera;

    [SerializeField]
    private float _ScannerDistance = 3f;

    [Header("Events Display")]
    [SerializeField]
    private TMPro.TextMeshPro _OnEventText;

    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip _ScannerSound;

    public void OnTriggerEnter(Collider other)
    {
        // If the player is around, trigger the event
        if (other.CompareTag("Player")) {
            _IsPlayerNearby = true;
            _PlayerCamera   = other.GetComponentInChildren<Camera>().transform;
            Debug.Log("Player is nearby");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // If the player is around, trigger the event
        if (other.CompareTag("Player")) {
            _IsPlayerNearby = false;
            _PlayerCamera   = null;
        }
    }

    public void Update()
    {
        if (_IsPlayerNearby && _PlayerCamera != null) {

            // Check if the player is looking at the scanner
            Ray ray = new(_PlayerCamera.position, _PlayerCamera.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _ScannerDistance)) {
                Debug.DrawRay(_PlayerCamera.position, _PlayerCamera.forward * _ScannerDistance, Color.red);

                if (hit.transform.gameObject.CompareTag("Scanner")) {

                    if (_OnEventText)
                        _OnEventText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E)) { // TODO: Change with InputSystem
                        _IsScanned = !_IsScanned;

                        AudioManager.Instance.PlaySFX(_ScannerSound, transform);

                        if (_IsScanned)
                            DoAction.Invoke(_IsScanned);
                        else
                            UndoAction.Invoke(_IsScanned);
                    }
                } else if (_OnEventText) {
                    _OnEventText.gameObject.SetActive(false);
                }
            } else if (_OnEventText) {
                _OnEventText.gameObject.SetActive(false);
            }
        }
    }
}
