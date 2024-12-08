using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AreaTriggerDoor : MonoBehaviour {

    [Header("Audio")]

    [SerializeField]
    private AudioClip _DoorOpenSound; // !< Door Open Sound

    [SerializeField]
    private AudioClip _DoorCloseSound; // !< Door Close Sound

    private Animator _Animator; // !< Door Animator

    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                return;

            // Trigger the door to open
            _Animator.SetTrigger("Open");

            // Play the door open sound
            AudioManager.Instance.PlaySFX(_DoorOpenSound, transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {

            // Trigger the door to close
            _Animator.SetTrigger("Closed");

            // Play the door close sound
            AudioManager.Instance.PlaySFX(_DoorCloseSound, transform);
        }
    }
}
