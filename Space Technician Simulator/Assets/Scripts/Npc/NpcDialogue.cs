using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NpcDialogue : MonoBehaviour
{
    [SerializeField] private float _DetectionRadius = 5f; // The radius to detect the player
    [SerializeField] private DialogueSO _DialogueSO; // The dialogues of a player to display
    [SerializeField] private NpcMovement _NpcMovement;
    [SerializeField] private TMP_Text _StartDialogueText;
    [SerializeField] private TMP_Text _NpcNameText;
    [SerializeField] private bool _CanLookPlayer;

    [SerializeField] private ActorSO _ActorsInfo; // (Contain the actorIndex)
    [SerializeField] private DialogueCamera _DialogueCamera; // (Contain the actorIndex)

    private bool _IsPlayerDetected = false;
    private bool _IsDialogueActive = false;
    private SphereCollider _SphereCollider;

    [Header("Events")]

    [SerializeField]
    private bool _DynamicDialogue = false;

    [SerializeField]
    private DialogueSO _DefaultDialogue; // Dialogue to put only on dynamic one.

    public Func<DialogueSO> OnNewDialogue;

    private void Awake()
    {
        _SphereCollider = gameObject.GetComponent<SphereCollider>();
        _SphereCollider.radius = _DetectionRadius;
        _SphereCollider.isTrigger = true;

        _StartDialogueText.text = "Press 'Enter' to talk";
        _StartDialogueText.gameObject.SetActive(false);


        // Get the actor name by index in actorSO

        // Find Actorname by actor id in

        if (_ActorsInfo != null
            && _ActorsInfo.Actors != null
            && _DialogueCamera.ActorIndex >= 0
            && _DialogueCamera.ActorIndex < _ActorsInfo.Actors.Length
            && _ActorsInfo.Actors[_DialogueCamera.ActorIndex] != null)
        {
            _NpcNameText.text = _ActorsInfo.Actors[_DialogueCamera.ActorIndex].actorName;
        }
        else
        {
            Debug.LogWarning("ActorSO is null. Ensure ActorSO is in the scene and initialized.");
        }
    }

    private void Start()
    {
        // Start the coroutine to register the event
        RegisterEvent();
    }

    private void Update()
    {
        if (_IsPlayerDetected && !_IsDialogueActive)
        {
            _StartDialogueText.gameObject.SetActive(true);
        }
        else
        {
            _StartDialogueText.gameObject.SetActive(false);
        }
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

        if (_DialogueSO == null) return;

        if (_DialogueSO.DialogueType == DialogueType.Manual)
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
            NotifyEndManualDialogue();
        }
    }

    public void StartDialogue()
    {
        if (_IsDialogueActive) return;

        //Debug.Log("Dialogue Started");

        _IsDialogueActive = true;

        if (_DynamicDialogue) {
            DialogueSO newDialog = OnNewDialogue?.Invoke();

            if (newDialog)
                _DialogueSO = newDialog;
            else
                _DialogueSO = _DefaultDialogue;
        }

        if (!_DialogueSO) return;

        if (_DialogueSO.DialogueType == DialogueType.Manual)
        {
            NotifyNewManualDialogue();
        }

        DialogueManager.Instance.StartDialogue(_DialogueSO);
    }

    public void NextMessageDialogue()
    {
        if (!_IsDialogueActive) return;

        DialogueManager.Instance.NextMessage(_DialogueSO.DialogueType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_DialogueSO == null) return;

        if (other.CompareTag("Player"))
        {
            _IsPlayerDetected = true;

            Debug.Log("OnTriggerEnter PLayer is detected");
            Debug.Log("OnTriggerEnter -> " + _IsDialogueActive);
            Debug.Log("OnTriggerEnter -> " + _DialogueSO.DialogueType);
            if (!_IsDialogueActive && _DialogueSO.DialogueType == DialogueType.Auto)
            {
                _IsDialogueActive = true;
                DialogueManager.Instance.StartDialogue(_DialogueSO);
                //Debug.Log("Player entered auto dialogue");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _IsDialogueActive && _DialogueSO.DialogueType == DialogueType.Auto && _CanLookPlayer)
        {
            _NpcMovement.LookAtPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_DialogueSO == null) return;

        if (other.CompareTag("Player"))
        {
            _IsPlayerDetected = false;

            if (_IsDialogueActive && _DialogueSO.DialogueType == DialogueType.Auto)
            {
                _IsDialogueActive = false;
                DialogueManager.Instance.EndDialogue(_DialogueSO.DialogueType);
                //Debug.Log("Player left auto dialogue");
            }
        }
    }

    // Notify the NPC to stop moving for manual dialogue
    public void NotifyNewManualDialogue()
    {
        _NpcMovement._CanMove = false;
        //Debug.Log("Stop Npc");
    }

    // Notify the NPC to start moving again after manual dialogue
    public void NotifyEndManualDialogue()
    {
        _NpcMovement._CanMove = true;
        //Debug.Log("Start Npc");
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }
}
