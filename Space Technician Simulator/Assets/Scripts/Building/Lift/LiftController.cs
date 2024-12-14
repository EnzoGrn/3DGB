using UnityEngine;

public class LiftController : MonoBehaviour {

    [Header("Lift Settings")]

    [SerializeField]
    private Transform _LiftContainer; // !< Reference to the lift container (Transform)

    [SerializeField]
    private Transform[] _Floors; // Positions of each floor

    [SerializeField]
    private EventTriggerDoor[] _Doors; // Doors of each floor

    [SerializeField]
    private float _Speed = 2f; // !< Speed of the elevator

    [Header("Lift Status")]

    [SerializeField]
    private int _CurrentFloor = 0; // !< Current floor of the elevator

    [SerializeField]
    private int _TargetFloor  = 0; // !< Target floor of the elevator

    [SerializeField]
    private bool _IsMoving = false; // !< Is the elevator moving?

    [SerializeField]
    private bool _Lock = false; // !< Is the elevator locked?

    [Header("Audio")]
    [SerializeField]
    private AudioClip _MoveSound; // !< Sound of the elevator moving

    private void Start()
    {
        if (_CurrentFloor != _TargetFloor) {
            CloseAllDoors();

            _IsMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (_IsMoving) {
            if (AudioManager.Instance != null && !AudioManager.Instance.IsPlayingSFX(_MoveSound))
                AudioManager.Instance.PlaySFX(_MoveSound, transform);
            MoveToTargetFloor();
        }
    }

    private void MoveToTargetFloor()
    {
        Vector3 targetPosition = new(_LiftContainer.position.x, _Floors[_TargetFloor].position.y, _LiftContainer.position.z);

        _LiftContainer.position = Vector3.MoveTowards(_LiftContainer.position, targetPosition, _Speed * Time.deltaTime);

        if (Vector3.Distance(_LiftContainer.position, targetPosition) < 0.01f) {
            _IsMoving     = false;
            _CurrentFloor = _TargetFloor;

            OpenDoor(_CurrentFloor);
        }
    }

    public void CallElevator(int floorIndex)
    {
        if (_Lock)
            return;
        if (!_IsMoving && floorIndex != _CurrentFloor) {
            CloseAllDoors();

            _TargetFloor  = floorIndex;
            _IsMoving     = true;
        }
    }

    public void OpenDoor(int floorIndex)
    {
        _Doors[floorIndex].OnTrigger(true);
    }

    public void CloseAllDoors()
    {
        foreach (EventTriggerDoor door in _Doors)
            door.OnTrigger(false);
    }

    public void PlayerIsInside(bool isInside)
    {
        if (isInside) {
            CloseAllDoors();

            if (_CurrentFloor == 0)
                _TargetFloor = 1;
            else
                _TargetFloor = 0;
        }

        ChangeFloor();
    }

    private void ChangeFloor()
    {
        if (_Lock)
            return;
        if (_CurrentFloor != _TargetFloor) {
            _IsMoving = true;
        }
    }

    public void LockElevator(bool isLocked)
    {
        _Lock = isLocked;
    }
}
