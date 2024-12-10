using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject questListItem;
    [SerializeField] private List<Quest> _quests = new List<Quest>();

    private void OnDisable()
    {
        QuestManager.Instance.OnQuestAdded -= UpdateQuestList;
        QuestManager.Instance.OnQuestRemoved -= RemoveQuest;
        Debug.Log("QuestUIManager disabled");
    }

    private void Start()
    {
        QuestManager.Instance.OnQuestAdded += UpdateQuestList;
        QuestManager.Instance.OnQuestRemoved += RemoveQuest;
        Debug.Log("QuestUIManager started");
    }

    private void UpdateQuestList(Quest quest)
    {
        _quests.Add(quest);

        quest.OnStepUpdated += UpdateQuestStep;
        GameObject questPanel = Instantiate(questListItem, transform);
        string questName = quest.DisplayName;
        Debug.Log(quest.QuestInfo.questSteps.Count + " steps in quest");
        if (quest.QuestInfo.questSteps.Count > 0 ) {
            questName = quest.QuestInfo.questSteps[quest.CurrentStepIndex].title;
        }
        questPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = questName;
        questPanel.name = quest.Id.ToString();
        Debug.Log(_quests.Count + " quests in list");
        Debug.Log("Quest list updated");
    }

    private void UpdateQuestStep(Quest quest, int stepIndex)
    {
        Transform questPanel = transform.Find(quest.Id.ToString());
        if (questPanel != null)
        {
            questPanel.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestInfo.questSteps[stepIndex].title;
        }
    }

    private void RemoveQuest(Quest quest)
    {
        _quests.Remove(quest);

        // Trouver le panneau associé
        Transform questPanel = transform.Find(quest.Id.ToString());
        if (questPanel != null)
        {
            Destroy(questPanel.gameObject);
        }

        Debug.Log(_quests.Count + " quests in list after removal");
        Debug.Log("Quest removed");
    }
}
