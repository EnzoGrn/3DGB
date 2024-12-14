using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : ARoom {

    [Header("Storage Elements")]

    [SerializeField]
    [Tooltip("The lights of the storage")]
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
