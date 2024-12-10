using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class AreaTriggerDoor : MonoBehaviour {

    [Header("Audio")]

    [SerializeField]
    private AudioClip _DoorOpenSound; // !< Door Open Sound

    [SerializeField]
    private AudioClip _DoorCloseSound; // !< Door Close Sound

    private Animator _Animator; // !< Door Animator

    [Header("Door Events")]

    [SerializeField]
    private UnityEvent _OnDoorOpen; // !< Event triggered when the door opens

    [SerializeField]
    private UnityEvent _OnDoorAlreadyOpen; // !< Event triggered when the door is already open

    [SerializeField]
    private UnityEvent _OnDoorClose; // !< Event triggered when the door closes

    [SerializeField]
    private UnityEvent _OnDoorAlreadyClose; // !< Event triggered when the door is already close

    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player is in the trigger
        if (other.CompareTag("Player")) {
            Close();
        }
    }

    public void Open(bool callEvent = true)
    {
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
            if (callEvent)
                _OnDoorAlreadyOpen?.Invoke();

            return;
        }

        // Trigger the door to open
        _Animator.SetTrigger("Open");

        // Play the door open sound
        AudioManager.Instance.PlaySFX(_DoorOpenSound, transform);

        // Trigger the door open event
        if (callEvent)
            _OnDoorOpen?.Invoke();
    }

    public void Close(bool callEvent = true)
    {
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) {
            if (callEvent)
                _OnDoorAlreadyClose?.Invoke();

            return;
        }

        // Trigger the door to close
        _Animator.SetTrigger("Closed");

        // Play the door close sound
        AudioManager.Instance.PlaySFX(_DoorCloseSound, transform);

        // Trigger the door close event
        if (callEvent)
            _OnDoorClose?.Invoke();
    }
}
