using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem {

    public class KeyDoorController : MonoBehaviour
    {
        private Animator _Animator;

        [Header("Door Settings")]
        private bool _DoorOpen = false;

        [Header("UI Settings")]
        [SerializeField]
        private int _TimeToShowUI = 1;

        [SerializeField]
        private GameObject _ShowDoorLockedUI = null;

        [SerializeField]
        private KeyInventory _KeyInventory = null;

        [SerializeField]
        private int _WaitTimer = 1;

        [SerializeField]
        private bool _PauseInteraction = false;

        [Header("Audio")]

        [SerializeField]
        private AudioClip _DoorOpenSound = null;

        [SerializeField]
        private AudioClip _DoorCloseSound = null;

        [SerializeField]
        private AudioClip _DoorLockedSound = null;

        [SerializeField]
        private AudioClip _DoorUnlockSound = null;

        private void Awake()
        {
            _Animator = GetComponent<Animator>();

            if (!_KeyInventory) {
                _KeyInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<KeyInventory>();

                if (!_KeyInventory)
                    Debug.LogError("KeyInventory is missing from the Player GameObject.");
            }
        }

        private IEnumerator PauseDoorInteraction(bool open)
        {
            _PauseInteraction = true;

            yield return new WaitForSeconds(_WaitTimer);

            _PauseInteraction = false;

            if (!open)
                AudioManager.Instance.PlaySFX(_DoorCloseSound, transform);
        }

        public void PlayAnimation()
        {
            if (_KeyInventory.hasBlueKey) {
                OpenDoor();
            } else {
                StartCoroutine(ShowDoorLocked());
            }
        }

        private void OpenDoor()
        {
            if (!_DoorOpen && !_PauseInteraction) {
                AudioManager.Instance.PlaySFX(_DoorUnlockSound, transform);

                _Animator.SetTrigger("Open");
                _DoorOpen = true;

                AudioManager.Instance.PlaySFX(_DoorOpenSound, transform);

                StartCoroutine(PauseDoorInteraction(true));
            } else {
                _Animator.SetTrigger("Closed");
                _DoorOpen = false;

                StartCoroutine(PauseDoorInteraction(false));
            }
        }

        private IEnumerator ShowDoorLocked()
        {
            AudioManager.Instance.PlaySFX(_DoorLockedSound, transform);

            _ShowDoorLockedUI.SetActive(true);

            yield return new WaitForSeconds(_TimeToShowUI);

            _ShowDoorLockedUI.SetActive(false);
        }
    }
}
