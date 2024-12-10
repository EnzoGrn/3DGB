using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text questLogText;
    [SerializeField] private List<Quest> _quests = new List<Quest>();

    private void OnEnable()
    {
        QuestManager.Instance.OnQuestAdded += UpdateQuestList;
        Debug.Log("QuestUIManager enabled");
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnQuestAdded -= UpdateQuestList;
        Debug.Log("QuestUIManager disabled");
    }

    private void UpdateQuestList(List<Quest> quests)
    {
        _quests = quests;
        Debug.Log(quests.Count + " quests in list");
        Debug.Log("Quest list updated");
    }

    private void Update()
    {
        questLogText.text = string.Empty;
        foreach (Quest quest in _quests)
        {
            Debug.Log("Quest: " + quest.DisplayName);
            questLogText.text += quest.DisplayName + "\n";
        }
    }
}
