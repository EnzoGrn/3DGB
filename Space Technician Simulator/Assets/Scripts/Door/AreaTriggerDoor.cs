using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerDoor : MonoBehaviour {

    private Animator _Animator; // !< Door Animator

    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {

            // Trigger the door to open
            _Animator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {

            // Trigger the door to close
            _Animator.SetTrigger("Closed");
        }
    }
}
