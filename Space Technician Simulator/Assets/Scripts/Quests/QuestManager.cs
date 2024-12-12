using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    public event System.Action<Quest> OnQuestAdded;
    public event System.Action<Quest> OnQuestRemoved;

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
        if (questInfo == null)
            return;
        Quest newQuest = new Quest(questInfo);

        foreach (QuestStep step in questInfo.questSteps) {
            if (step.prefab == null)
                continue;
            GameObject stepInstance = Instantiate(step.prefab, step.prefab.transform.position, Quaternion.identity);
            NPCInteractionHandler nPCInteractionHandler = stepInstance.GetComponent<NPCInteractionHandler>();

            if (nPCInteractionHandler != null)
                nPCInteractionHandler.Initialize(newQuest);

            // -- Circuit Vital Quest Step --
            CircuitVitalQuest circuitVital = stepInstance.GetComponent<CircuitVitalQuest>();

            if (circuitVital != null)
                circuitVital.Initialize(newQuest);
            // -------------------------------
        }
        newQuest.OnQuestCompleted += HandleQuestCompleted;
        activeQuests.Add(newQuest);
        OnQuestAdded?.Invoke(newQuest);
    }

    private void HandleQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        OnQuestRemoved?.Invoke(quest);
        Debug.Log("Quest completed: " + quest.DisplayName);
        //OnQuestAdded?.Invoke(quest);
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
