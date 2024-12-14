using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : ARoom {

    [Header("Garden Elements")]

    [SerializeField]
    [Tooltip("The lights of the garden")]
    private GameObject[] _Lights;

    [SerializeField]
    [Tooltip("The doors of the garden")]
    public EventTriggerDoor[] _Doors;

    public void OpenDoor()
    {
        for (int i = 0; i < _Doors.Length; i++)
            _Doors[i].OnTrigger(true);
    }

    public void CloseDoor()
    {
        for (int i = 0; i < _Doors.Length; i++)
            _Doors[i].OnTrigger(false);
    }

    public override void SetLight(bool isLightOn)
    {
        // -- Put the lights on or off, when interuptor change state
        for (int i = 0; i < _Lights.Length; i++)
            _Lights[i].SetActive(isLightOn);

        // -- Play or stop the ambient sound, when interuptor change state
        if (isLightOn)
            OpenDoor();
        else
            CloseDoor();
    }

    public void Awake()
    {
        OnElectricityChange += SetLight;
    }
}
