using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorManager : ARoom {

    [Header("Corridor Elements")]

    [SerializeField]
    [Tooltip("The lights of the corridor")]
    private GameObject[] _Lights;

    [SerializeField]
    [Tooltip("The door of the corridor")]
    public AreaTriggerDoor[] _AreaDoor;

    [SerializeField]
    [Tooltip("The door of the corridor")]
    public EventTriggerDoor[] _TriggerDoor;

    [SerializeField]
    [Tooltip("The elevator of the corridor")]
    public LiftController[] _Elevator;

    [SerializeField]
    [Tooltip("The alarm sound of the spaceship")]
    private AudioSource _AmbientSound;

    public override void SetLight(bool isLightOn)
    {
        // -- Lock or unlock the doors, when interuptor change state
        for (int i = 0; i < _AreaDoor.Length; i++)
            _AreaDoor[i].LockDoor(!isLightOn);
        for (int i = 0; i < _TriggerDoor.Length; i++)
            _TriggerDoor[i].LockDoor(!isLightOn);
        for (int i = 0; i < _Elevator.Length; i++)
            _Elevator[i].LockElevator(!isLightOn);

        // -- Alarm
        if (isLightOn == false)
            StartAlarm();
        else
            StopAlarm();
    }

    private void StartAlarm()
    {
        _AmbientSound.Play();

        // -- Infinite coroutine to make the lights blink
        StartCoroutine(Alarm());
    }

    private IEnumerator Alarm()
    {
        while (IsLightOn == false) {
            for (int i = 0; i < _Lights.Length; i++)
                _Lights[i].SetActive(!_Lights[i].activeSelf);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StopAlarm()
    {
        _AmbientSound.Stop();

        for (int i = 0; i < _Lights.Length; i++)
            _Lights[i].SetActive(true);
        _TriggerDoor[0].OnTrigger(true);
    }

    public void Awake()
    {
        OnElectricityChange += SetLight;
    }
}
