using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseController : MonoBehaviour {

    public event Action OnFusePickup = null;

    public void ObjectInteraction()
    {
        OnFusePickup?.Invoke();
    }

    #region Waypoint

    public GameObject WaypointPrefab;

    [SerializeField]
    private GameObject _WaypointParent;

    [SerializeField]
    private GameObject _Waypoint;

    private Waypoint _WaypointScript;

    public void InitWaypoint()
    {
        _WaypointScript = GetComponent<Waypoint>();

        if (!_WaypointScript)
            Debug.LogError("Waypoint script is missing from the FuseController.");
        _WaypointParent = GameObject.FindGameObjectWithTag("WaypointsParent");

        if (!_WaypointParent)
            Debug.LogError("WaypointsParent is missing from the scene.");
        if (_WaypointParent && WaypointPrefab)
            _Waypoint = Instantiate(WaypointPrefab, _WaypointParent.transform);
        _WaypointScript.SetWaypointUI(_Waypoint);
        _WaypointScript.isDisabled = true;
    }

    public void Set()
    {
        _WaypointScript.isDisabled = false;
    }

    public void Delete()
    {
        _WaypointScript.isDisabled = false;
        _WaypointScript.DestroyWaypoint();
    }

    #endregion
}
