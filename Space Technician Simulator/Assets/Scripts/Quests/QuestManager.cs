using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    public event System.Action<List<Quest>> OnQuestAdded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuest(QuestInfoSO questInfo)
    {
        if (questInfo == null) return;
        Debug.Log("Starting quest: " + questInfo.displayName);
        Quest newQuest = new Quest(questInfo);
        foreach (GameObject step in questInfo.questStepPrefabs)
        {
            NPCInteractionHandler nPCInteractionHandler = step.GetComponent<NPCInteractionHandler>();
            if (nPCInteractionHandler != null)
            {
                nPCInteractionHandler.Initialize(newQuest);
            }
        }
        newQuest.OnQuestCompleted += HandleQuestCompleted;
        activeQuests.Add(newQuest);
        OnQuestAdded?.Invoke(activeQuests);
    }

    private void HandleQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        Debug.Log("Quest completed: " + quest.DisplayName);
        OnQuestAdded?.Invoke(activeQuests);
        completedQuests.Add(quest);
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(activeQuests);
    }

    public List<Quest> GetCompletedQuests()
    {
        return new List<Quest>(completedQuests);
    }
}
