using UnityEngine;

public class LiftContainer : MonoBehaviour {

    [Header("Lift Controller")]
    [SerializeField]
    private LiftController _LiftController; // !< Reference to the lift controller

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered the lift container");
            _LiftController.PlayerIsInside(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            _LiftController.PlayerIsInside(false);
        }
    }
}
