using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private float _DetectionRadius = 3f; // The radius to detect the player

    private SphereCollider _SphereCollider;
    private NpcDialogue _NpcDialogue;

    public bool IsManualDialogueActive;

    private void Awake()
    {
        _SphereCollider = gameObject.AddComponent<SphereCollider>();
        _SphereCollider.radius = _DetectionRadius;
        _SphereCollider.isTrigger = true;
        IsManualDialogueActive = false;
        _NpcDialogue = null;
    }

    private void Update()
    {
        // Check enter key input
        if (Input.GetKeyDown(KeyCode.T) && _NpcDialogue != null && !IsManualDialogueActive)
        {
            _NpcDialogue.StartDialogue();
            IsManualDialogueActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.T) && _NpcDialogue != null && IsManualDialogueActive)
        {
            // Continue the dialogue
            _NpcDialogue.NextMessageDialogue();
        }
    }

    public void EndDialogue()
    {
        IsManualDialogueActive = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _DetectionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter + " + other.name);

        if (_NpcDialogue != null) return;

        if (other.CompareTag("ManualDialogue"))
        {
            Debug.Log("ManualDialogue Detected");

            _NpcDialogue = other.GetComponent<NpcDialogue>();
            // Chnage the color of the NPC
            _NpcDialogue.ChangeColor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_NpcDialogue == null) return;

        if (other.CompareTag("ManualDialogue"))
        {
            Debug.Log("ManualDialogue Left");

            _NpcDialogue.ChangeColor(false);
            _NpcDialogue = null;
        }
    }
}
