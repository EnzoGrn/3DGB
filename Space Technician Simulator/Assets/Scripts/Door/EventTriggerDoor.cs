using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerDoor : MonoBehaviour {

    private Animator _Animator; // !< Door Animator

    [Header("Door State")]
    [SerializeField]
    private bool _IsOpen = false; // !< Door state

    private void Start()
    {
        _Animator = GetComponent<Animator>();

        if (_IsOpen)
            OnTrigger(_IsOpen);
    }

    public void OnTrigger(bool open)
    {
        if (open)
            _Animator.SetTrigger("Open");
        else
            _Animator.SetTrigger("Closed");
    }
}
