 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _Name;
    [SerializeField] private ActorSO _Actors;
    [SerializeField] private TMP_Text _Dialogue;
    [SerializeField] private GameObject _DialogueBox;
    [SerializeField] private GameObject _HintKeyBox;

    [Header("PLayer")]
    [SerializeField] private GameObject _Player;

    [Header("Dialogue Camera")]
    [SerializeField] private Camera _PlayerCamera3rdPersonn;
    [SerializeField] private Camera _PlayerCamera1stPerson;
    [SerializeField] private DialogueCamera[] _DialogueCamera;
    private DialogueCamera _DialogueCameraActive;

    Message[] _Messages;

    private int _ActiveMessageIndex;
    private bool _IsDialogueActive;

    private Coroutine _MessageCoroutine;

    public static DialogueManager Instance;

    // Action to know when the dialogue is ended
    public Action OnDialogueEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        // Deactivate the dialogue box
        if (_DialogueBox) _DialogueBox.SetActive(false);
        if (_HintKeyBox) _HintKeyBox.SetActive(false);
        _IsDialogueActive = false;
        _MessageCoroutine = null;
        _ActiveMessageIndex = 0;
    }

    public void StartDialogue(DialogueSO dialogues)
    {
        if (!dialogues || _IsDialogueActive) return;

        Debug.Log("Dialogue Started + " + dialogues.Messages.Length);

        // Activate the dialogue box
        _DialogueBox.SetActive(true);
        _Messages = dialogues.Messages;
        _ActiveMessageIndex = 0;
        _IsDialogueActive = true;

        if (dialogues.DialogueType == DialogueType.ManualDialogue)
        {
            _HintKeyBox.SetActive(true);
        }

        DisplayMessage(dialogues.DialogueType);
    }

    private DialogueCamera FindDialogueCamera(int actorId)
    {
        Debug.Log("Lenght of _DialogueCamera " + _DialogueCamera.Length);

        foreach (DialogueCamera dialogueCamera in _DialogueCamera)
        {

            Debug.Log("try to get dialogueCamera !!!!!!!!!!!! " + dialogueCamera.ActorIndex);

        }

            foreach (DialogueCamera dialogueCamera in _DialogueCamera)
        {
            Debug.Log("try to get dialogueCamera " + dialogueCamera.ActorIndex);

            if (dialogueCamera.Camera == null)
            {
                Debug.LogError("Dialogue Camera is null");
            }

            if (dialogueCamera.ActorIndex == actorId)
            {
                return dialogueCamera;
            }
        }

        Debug.LogWarning("Dialogue Camera not found for actorId -> " + actorId);

        return null;
    }

    private void HandleDialogueCamera(Message message)
    {
        if (_DialogueCameraActive != null && _DialogueCameraActive.ActorIndex == message.actorId) return;

        Debug.Log("FindDialogueCamera message.actorId -> " + message.actorId);

        DialogueCamera dialogueCamera = FindDialogueCamera(message.actorId);

        if (_DialogueCameraActive != null)
        {
            _DialogueCameraActive.DisableDialogueCamera();
        }
        _DialogueCameraActive = dialogueCamera;

        Debug.Log("Dialogue -> " + _DialogueCameraActive);
        Debug.Log("_Player -> " + _Player.transform);

        if (message.toLookId == 0)
        {
            _DialogueCameraActive.EnableDialogueCamera(_Player.transform);
        }
        else
        {
            DialogueCamera dialogueCameraToLook = FindDialogueCamera(message.toLookId);

            if (dialogueCameraToLook != null)
            {
                _DialogueCameraActive.EnableDialogueCamera(dialogueCameraToLook.transform);

            }
        }
        Debug.Log("Camera Enabled");
        return;
    }

    private void DisplayMessage(DialogueType dialogueType)
    {
        // Get the current message && actor by id
        Message message = _Messages[_ActiveMessageIndex];
        Actor actor = _Actors.Actors[message.actorId];

        if (actor == null) return;

        if (dialogueType == DialogueType.ManualDialogue)
        {
            HandleDialogueCamera(message);
        }

        if (actor.actorName != _Name.text)
        {
            _Name.text = actor.actorName;

            if (message.actorId == 0)
            {
                _Name.color = Color.green;
            }
            else
            {
                // set this color #149CFF
                _Name.color = new Color(0.08235294f, 0.6117647f, 1f);
            }
        }

        // Cancel the coroutine if it's already running
        if (_MessageCoroutine != null) StopCoroutine(_MessageCoroutine);
        _MessageCoroutine = StartCoroutine(TypeMessage(message.message, dialogueType));
    }

    private IEnumerator TypeMessage(string message, DialogueType dialogueType)
    {
        _Dialogue.text = "";
        foreach (char letter in message.ToCharArray())
        {
            _Dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        if (dialogueType == DialogueType.AutoDialogue)
        {
            yield return new WaitForSeconds(2f);
            NextMessage(dialogueType);
        }
    }

    public void NextMessage(DialogueType dialogueType)
    {
        _ActiveMessageIndex++;
        if (_ActiveMessageIndex < _Messages.Length)
        {
            DisplayMessage(dialogueType);
        }
        else
        {
            EndDialogue(dialogueType);
        }
    }

    public void EndDialogue(DialogueType dialogueType)
    {
        Debug.Log("Dialogue Ended");

        _IsDialogueActive = false;
        _DialogueBox.SetActive(false);
        _HintKeyBox.SetActive(false);

        if (dialogueType == DialogueType.ManualDialogue)
        {
            if (_DialogueCameraActive != null)
            {
                _DialogueCameraActive.DisableDialogueCamera();
                _DialogueCameraActive = null;
                _PlayerCamera3rdPersonn.enabled = true;
            }
        }

        OnDialogueEnded?.Invoke();
    }

    public void SetDialogueActive(bool value)
    {
        _IsDialogueActive = value;
    }
}

[System.Serializable]
public enum DialogueType
{
    AutoDialogue,
    ManualDialogue
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
    public int toLookId;
}

[System.Serializable]
public class Actor
{
    public int actorId;
    public string actorName;
}
