using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    /*
    * If the dialogue is a manual dialogue, then if there is an id to talk in the message,
    * the dialogue manager must have the dialogueCamera with the same id registered.
    */

    [Header("Dialogue Box")]
    [SerializeField] public Message[] Messages;
    [SerializeField] public DialogueType DialogueType;
}
