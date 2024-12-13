using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EventTriggerDoor : MonoBehaviour {

    private Animator _Animator; // !< Door Animator

    [Header("Door State")]
    [SerializeField]
    private bool _IsOpen = false; // !< Door state

    [SerializeField]
    private bool _IsLocked = false; // !< Door state

    [Header("Audio")]
    [SerializeField]
    private AudioClip _DoorOpenSound; // !< Door Open Sound

    [SerializeField]
    private AudioClip _DoorCloseSound; // !< Door Close Sound

    private void Awake()
    {
        _Animator = GetComponent<Animator>();

        if (_IsOpen) {
            _IsOpen = false;

            OnTrigger(true);
        }
    }

    public void OnTrigger(bool open)
    {
        if (_IsOpen == open || _IsLocked)
            return;
        _IsOpen = open;

        if (open && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
            _Animator.SetTrigger("Open");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(_DoorOpenSound, transform);
        } else {
            _Animator.SetTrigger("Closed");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(_DoorCloseSound, transform);
        }
    }

    public void LockDoor(bool locked)
    {
        _IsLocked = locked;
    }
}
