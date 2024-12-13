using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom {

    void ElectricityOn();
    void ElectricityOff();

    void SetLight(bool isLightOn);
}

public class ARoom : MonoBehaviour, IRoom {

    [Header("Room Settings")]

    [SerializeField]
    [Tooltip("The time to wait before the lights turn off")]
    private bool _IsLightOn = true;

    public Action<bool> OnElectricityChange = null;

    public bool IsLightOn
    {
        get { return _IsLightOn; }
        set
        {
            _IsLightOn = value;

            OnElectricityChange?.Invoke(value);
        }
    }

    public virtual void ElectricityOn()
    {
        IsLightOn = true;
    }

    public virtual void ElectricityOff()
    {
        IsLightOn = false;
    }

    public virtual void SetLight(bool isLightOn)
    {
        _IsLightOn = isLightOn;
    }
}
