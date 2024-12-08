using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerDoor : MonoBehaviour {

    private Animator _Animator; // !< Door Animator

    [Header("Door State")]
    [SerializeField]
    private bool _IsOpen = false; // !< Door state

    [Header("Audio")]

    [SerializeField]
    private AudioClip _DoorOpenSound; // !< Door Open Sound

    [SerializeField]
    private AudioClip _DoorCloseSound; // !< Door Close Sound

    private void Start()
    {
        _Animator = GetComponent<Animator>();

        if (_IsOpen)
            OnTrigger(_IsOpen);
    }

    public void OnTrigger(bool open)
    {
        _IsOpen = open;

        if (open) {
            _Animator.SetTrigger("Open");

            AudioManager.Instance.PlaySFX(_DoorOpenSound);
        } else {
            _Animator.SetTrigger("Closed");

            AudioManager.Instance.PlaySFX(_DoorCloseSound);
        }
    }
}
