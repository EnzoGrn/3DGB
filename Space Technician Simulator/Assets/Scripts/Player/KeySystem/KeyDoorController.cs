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

        private void Awake()
        {
            _Animator = GetComponent<Animator>();

            if (!_KeyInventory) {
                _KeyInventory = GameObject.Find("Player").GetComponent<KeyInventory>();

                if (!_KeyInventory)
                    Debug.LogError("KeyInventory is missing from the Player GameObject.");
            }
        }

        private IEnumerator PauseDoorInteraction()
        {
            _PauseInteraction = true;

            yield return new WaitForSeconds(_WaitTimer);

            _PauseInteraction = false;
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
                _Animator.SetTrigger("Open");
                _DoorOpen = true;

                StartCoroutine(PauseDoorInteraction());
            } else {
                _Animator.SetTrigger("Closed");
                _DoorOpen = false;

                StartCoroutine(PauseDoorInteraction());
            }
        }

        private IEnumerator ShowDoorLocked()
        {
            _ShowDoorLockedUI.SetActive(true);

            yield return new WaitForSeconds(_TimeToShowUI);

            _ShowDoorLockedUI.SetActive(false);
        }
    }
}
