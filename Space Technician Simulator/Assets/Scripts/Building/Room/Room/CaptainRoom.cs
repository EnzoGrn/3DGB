using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainRoom : ARoom {

    [Header("Captain room Elements")]

    [SerializeField]
    [Tooltip("The lights of the captain room")]
    private GameObject[] _Lights;

    [SerializeField]
    [Tooltip("The door of the captain room")]
    private AreaTriggerDoor[] _AreaDoor;

    public override void SetLight(bool isLightOn)
    {
        // -- Put the lights on or off, when interuptor change state
        for (int i = 0; i < _Lights.Length; i++)
            _Lights[i].SetActive(IsLightOn);

        // -- Block or unblock the door, when interuptor change state
        for (int i = 0; i < _AreaDoor.Length; i++)
            _AreaDoor[i].LockDoor(!IsLightOn);
    }

    public void Awake()
    {
        OnElectricityChange += SetLight;
    }
}
