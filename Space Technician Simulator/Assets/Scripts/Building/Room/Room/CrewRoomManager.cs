using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewRoomManager : ARoom {

    [Header("Crew Room Elements")]

    [SerializeField]
    [Tooltip("The lights of the crew room")]
    private GameObject[] _Lights;

    public override void SetLight(bool isLightOn)
    {
        // -- Put the lights on or off, when interuptor change state
        for (int i = 0; i < _Lights.Length; i++)
            _Lights[i].SetActive(IsLightOn);
    }

    public void Awake()
    {
        OnElectricityChange += SetLight;
    }
}
