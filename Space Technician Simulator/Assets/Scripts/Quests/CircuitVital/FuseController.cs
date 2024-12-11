using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuseController : MonoBehaviour {

    public UnityEvent OnFusePickup;

    public void ObjectInteraction()
    {
        OnFusePickup.Invoke();

        Destroy(gameObject);
    }
}
