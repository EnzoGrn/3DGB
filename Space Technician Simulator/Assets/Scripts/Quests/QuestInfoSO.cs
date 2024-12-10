using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    public string title; // Le titre de l'�tape
    [TextArea] public string completionMessage; // Le message affich� � la fin de l'�tape
    public GameObject prefab; // Le prefab associ� � l'�tape (facultatif)
}

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    public string description;

    [Header("Quest Messages")]
    public string startMessage; // Message de d�but
    public string endMessage;   // Message de fin

    [Header("Steps")]
    public List<QuestStep> questSteps = new List<QuestStep>();

    // ensure the id is always the name of the Scriptable Object asset
    private void OnValidate()
    {
    #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    #endif
    }
}