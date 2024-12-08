using UnityEngine;

public class LiftContainer : MonoBehaviour {

    [Header("Lift Controller")]
    [SerializeField]
    private LiftController _LiftController; // !< Reference to the lift controller

    [Header("Lift Status")]
    [SerializeField]
    private bool _PlayerInside = false; // !< Is the player inside the elevator?

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            _PlayerInside = true;

            _LiftController.PlayerIsInside(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            _PlayerInside = false;

            _LiftController.PlayerIsInside(false);
        }
    }
}
