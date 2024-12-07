using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    [SerializeField] private float _DetectionRadius = 5f; // The radius to detect the player
    [SerializeField] private DialogueSO _DialogueSO; // The dialogues of a player to display

    private bool _IsPlayerDetected = false;
    private bool _IsDialogueActive = false;

    private SphereCollider _SphereCollider;
    private GameObject _Player;

    private void Awake()
    {
        _SphereCollider = gameObject.AddComponent<SphereCollider>();
        _SphereCollider.radius = _DetectionRadius;
        _SphereCollider.isTrigger = true;

        // Find the player
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        // Start the coroutine to register the event
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded += OnDialogueEnd;
        }
        else
        {
            Debug.LogError("DialogueManager.Instance is null. Ensure DialogueManager is in the scene and initialized.");
        }
    }

    private void UnregisterEvent()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnd;
        }
    }

    private void OnDialogueEnd()
    {
        Debug.Log("Dialogue Ended Event");
        _IsDialogueActive = false;

        if (_DialogueSO.DialogueType == DialogueType.ManualDialogue)
        {
            // Find GameObejct "DialogueTrigger" and then the script DialogueTrigger
            GameObject dialogueTrigger = GameObject.Find("DialogueTrigger");
            
            if (dialogueTrigger == null)
            {
                Debug.LogWarning("DialogueTrigger not found.");
                return;
            }

            DialogueTrigger dialogueTriggerScript = dialogueTrigger.GetComponent<DialogueTrigger>();
            
            if (dialogueTriggerScript == null)
            {
                Debug.LogWarning("DialogueTrigger script not found.");
                return;
            }

            dialogueTriggerScript.EndDialogue();
        }
    }

    public void ChangeColor(bool enter)
    {
        // Change the color of the NPC
        if (enter)
            GetComponent<MeshRenderer>().material.color = Color.green;
        else
            GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void StartDialogue()
    {
        if (_IsDialogueActive) return;

        Debug.Log("Dialogue Started");

        _IsDialogueActive = true;

        DialogueManager.Instance.StartDialogue(_DialogueSO);
    }

    public void NextMessageDialogue()
    {
        if (!_IsDialogueActive) return;

        DialogueManager.Instance.NextMessage(_DialogueSO.DialogueType);
    }

    private void LookAtPlayer()
    {
        if (!_IsPlayerDetected) return;

        // Rotate the NPC to face the player
        Vector3 direction = _Player.transform.position - transform.position;
        direction.y = 0;
        direction.z = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_IsDialogueActive && _DialogueSO.DialogueType == DialogueType.AutoDialogue)
        {
            Debug.Log("Player detected");

            _IsPlayerDetected = true;
            _IsDialogueActive = true;
        
            DialogueManager.Instance.StartDialogue(_DialogueSO);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LookAtPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _IsDialogueActive && _DialogueSO.DialogueType == DialogueType.AutoDialogue)
        {
            Debug.Log("Player left");

            _IsPlayerDetected = false;
            _IsDialogueActive = false;

            DialogueManager.Instance.EndDialogue();
        }
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }
}
