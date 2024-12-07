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

    Message[] _Messages;

    private int _ActiveMessageIndex;
    private bool _IsDialogueActive;
    
    private Coroutine _MessageCoroutine;

    public static DialogueManager Instance;

    // Action to know when the dialogue is ended
    public Action OnDialogueEnded;

    private void Awake()
    {
        // Deactivate the dialogue box
        _DialogueBox.SetActive(false);
        _IsDialogueActive = false;
        _MessageCoroutine = null;
        _ActiveMessageIndex = 0;
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void StartDialogue(DialogueSO dialogues)
    {
        if (_IsDialogueActive) return;

        Debug.Log("Dialogue Started + " + dialogues.Messages.Length);

        // Activate the dialogue box
        _DialogueBox.SetActive(true);
        _Messages = dialogues.Messages;
        _ActiveMessageIndex = 0;
        _IsDialogueActive = true;

        DisplayMessage(dialogues.DialogueType);
    }

    private void DisplayMessage(DialogueType dialogueType)
    {
        // Get the current message && actor by id
        Message message = _Messages[_ActiveMessageIndex];
        Actor actor = _Actors.Actors[message.actorId];

        if (actor == null) return;
            
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
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        Debug.Log("Dialogue Ended");

        _IsDialogueActive = false;
        _DialogueBox.SetActive(false);

        OnDialogueEnded?.Invoke();
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
}

[System.Serializable]
public class Actor
{
    public int actorId;
    public string actorName;
}
