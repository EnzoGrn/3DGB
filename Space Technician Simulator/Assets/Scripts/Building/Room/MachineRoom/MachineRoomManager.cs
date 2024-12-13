using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineRoomManager : ARoom {

    [Header("Machine Room Elements")]

    [SerializeField]
    [Tooltip("The lights of the machine room")]
    private GameObject[] _Lights;

    [SerializeField]
    [Tooltip("The door of the machine room")]
    public EventTriggerDoor _Door;

    [SerializeField]
    [Tooltip("The ambient sound of the machine room")]
    private AudioSource _AmbientSound;

    public void OpenDoor()
    {
        _Door.OnTrigger(true);
    }

    public override void SetLight(bool isLightOn)
    {
        // -- Put the lights on or off, when interuptor change state
        for (int i = 0; i < _Lights.Length; i++)
            _Lights[i].SetActive(isLightOn);

        // -- Play or stop the ambient sound, when interuptor change state
        if (isLightOn)
            _AmbientSound.Play();
        else
            _AmbientSound.Stop();
    }
    public void Awake()
    {
        OnElectricityChange += SetLight;
    }
}
