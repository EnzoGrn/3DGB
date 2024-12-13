using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineRoomManager : MonoBehaviour {

    [Header("Machine Room Elements")]

    [SerializeField]
    [Tooltip("The lights of the machine room")]
    private GameObject[] _Lights;

    [SerializeField]
    [Tooltip("The door of the machine room")]
    public EventTriggerDoor _Door;

    [Header("Machine Room Settings")]

    [SerializeField]
    [Tooltip("The time to wait before the lights turn off")]
    private bool _IsLightOn = true;

    public bool IsLightOn
    {
        get { return _IsLightOn; }
        set
        {
            _IsLightOn = value;

            // -- Put the lights on or off, when interuptor change state
            for (int i = 0; i < _Lights.Length; i++)
                _Lights[i].SetActive(_IsLightOn);
        }
    }

    public void OpenDoor()
    {
        _Door.OnTrigger(true);
    }

    public void ElectricityOn()
    {
        IsLightOn = true;
    }

    public void ElectricityOff()
    {
        IsLightOn = false;
    }
}
