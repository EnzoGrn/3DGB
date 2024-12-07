using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    // Set a max of 3 options
    // private const int MaxOptions = 3;

    //[Header("Dialogue Box")]
    [SerializeField] public Message[] Messages;
    [SerializeField] public DialogueType DialogueType;

    //[Header("Options Box")]
    //[SerializeField] public Option[] Options;
    //[SerializeField] public DialogueSO[] OptionDialogues;

    //private void OnValidate()
    //{
    //    // Limit the size of the Options array
    //    if (Options != null && Options.Length > MaxOptions)
    //    {
    //        Debug.LogWarning($"Trimming Options array to the maximum allowed size of {MaxOptions}.");
    //        System.Array.Resize(ref Options, MaxOptions);
    //    }

    //    // Limit the size of the OptionDialogues array
    //    if (OptionDialogues != null && OptionDialogues.Length > MaxOptions)
    //    {
    //        Debug.LogWarning($"Trimming OptionDialogues array to the maximum allowed size of {MaxOptions}.");
    //        System.Array.Resize(ref OptionDialogues, MaxOptions);
    //    }
    //}
}
