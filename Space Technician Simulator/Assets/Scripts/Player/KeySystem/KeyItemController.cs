using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem {

    public class KeyItemController : MonoBehaviour {

        [Header("Blue Key Settings")]
        [SerializeField]
        private bool _BlueDoor = false;

        [SerializeField]
        private bool _BlueKey = false;

        [Header("Red Key Settings")]
        [SerializeField]
        private bool _RedDoor = false;

        [SerializeField]
        private bool _RedKey = false;

        [Header("Yellow Key Settings")]
        [SerializeField]
        private bool _YellowDoor = false;

        [SerializeField]
        private bool _YellowKey = false;

        [Header("Door Settings")]
        [SerializeField]
        private KeyDoorController _DoorObject;

        [Header("Key Inventory")]
        [SerializeField]
        private KeyInventory _KeyInventory = null;

        private void Start()
        {
            if (!_KeyInventory) {
                _KeyInventory = GameObject.Find("Player").GetComponent<KeyInventory>();

                if (!_KeyInventory)
                    Debug.LogError("KeyInventory is missing from the Player GameObject.");
            }

            if ((_BlueDoor || _RedDoor || _YellowDoor) && !_DoorObject)
                _DoorObject = GetComponent<KeyDoorController>();
        }

        public void ObjectInteraction()
        {
            Debug.Log("ObjectInteraction");
            if (_BlueDoor || _RedDoor || _YellowDoor) {
                _DoorObject.PlayAnimation();
            } else if (_BlueKey) {
                _KeyInventory.hasBlueKey = true;

                gameObject.SetActive(false);
            } else if (_RedKey) {
                _KeyInventory.hasRedKey = true;

                gameObject.SetActive(false);
            } else if (_YellowKey) {
                _KeyInventory.hasYellowKey = true;

                gameObject.SetActive(false);
            }
        }
    }
}
