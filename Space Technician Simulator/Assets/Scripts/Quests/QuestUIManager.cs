using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _Dialogue;
    [SerializeField] private TMP_Text _Name;
    [SerializeField] private List<Quest> _quests = new List<Quest>();
    [SerializeField] private GameObject questListItem;
    [SerializeField] private GameObject questHUD;
    [SerializeField] private GameObject _DialogueBox;
    [SerializeField] private GameObject _NarratorPortrait;

    [Header("Custom parameters")]
    public UnityEvent OnDialogueEnded;

    private void OnDisable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestAdded -= UpdateQuestList;
            QuestManager.Instance.OnQuestRemoved -= RemoveQuest;
            Debug.Log("QuestUIManager disabled");
        }
    }

    private void Start()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestAdded += UpdateQuestList;
            QuestManager.Instance.OnQuestRemoved += RemoveQuest;
            Debug.Log("QuestUIManager started");
        }
    }

    private void FixedUpdate()
    {
        if (questHUD.activeSelf == false) {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
    }

    public void StartDialogue(DialogueSO dialogues)
    {
        DialogueManager.Instance.StartDialogue(dialogues);

        _NarratorPortrait.SetActive(true);

        DialogueManager.Instance.OnDialogueEnded += OnDialogueEnd;
    }

    private void UpdateQuestList(Quest quest)
    {
        _quests.Add(quest);

        quest.OnStepUpdated += UpdateQuestStep;
        GameObject questPanel = Instantiate(questListItem, transform);
        string questName = quest.DisplayName;
        Debug.Log(quest.QuestInfo.questSteps.Count + " steps in quest");
        if (quest.QuestInfo.questSteps.Count > 0)
        {
            questName = quest.QuestInfo.questSteps[quest.CurrentStepIndex].title;
        }
        StartDialogue(quest.QuestInfo.startDialogue);
        questPanel.transform.GetComponentInChildren<TextMeshProUGUI>().text = questName;
        questPanel.name = quest.Id.ToString();
        Animator questAnimator = questPanel.GetComponent<Animator>();
        questAnimator.SetBool("Active", false);
        Debug.Log(_quests.Count + " quests in list");
        Debug.Log("Quest list updated");
    }

    private void UpdateQuestStep(Quest quest, int stepIndex)
    {
        Transform questPanel = transform.Find(quest.Id.ToString());
        if (questPanel != null)
        {
            Animator questAnimator = questPanel.GetComponent<Animator>();
            questAnimator.SetBool("Active", true);
            StartDialogue(quest.QuestInfo.questSteps[stepIndex - 1].completionDialogue);
            StartCoroutine(ResetQuestPanel(questPanel.gameObject, quest));
        }
    }

    private IEnumerator ResetQuestPanel(GameObject questPanel, Quest quest)
    {
        yield return new WaitForSeconds(1.0f);
        Animator questAnimator = questPanel.GetComponent<Animator>();
        questAnimator.SetBool("Active", false);
        string questName = quest.DisplayName;
        if (quest.QuestInfo.questSteps.Count > 0)
        {
            questName = quest.QuestInfo.questSteps[quest.CurrentStepIndex].title;
        }
        questPanel.GetComponentInChildren<TextMeshProUGUI>().text = questName;
    }

    private void RemoveQuest(Quest quest)
    {
        _quests.Remove(quest);

        // Trouver le panneau associ�
        Transform questPanel = transform.Find(quest.Id.ToString());
        if (questPanel != null)
        {
            Animator questAnimator = questPanel.GetComponent<Animator>();
            questAnimator.SetBool("Active", true);
            StartCoroutine(DestroyQuestPanel(questPanel.gameObject));
        }

        if (_quests.Count == 0)
        {
            questHUD.SetActive(false);
        }
        else
        {
            questHUD.SetActive(true);
        }

        if (quest.QuestInfo.endDialogue)
            StartDialogue(quest.QuestInfo.endDialogue);

        Debug.Log(_quests.Count + " quests in list after removal");
        Debug.Log("Quest removed");
    }

    private IEnumerator DestroyQuestPanel(GameObject questPanel)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(questPanel);
    }

    private void HideNarrator()
    {
        _NarratorPortrait.SetActive(false);
    }

    private void OnDialogueEnd()
    {
        HideNarrator();
        CallCustomEvent();
    }

    private void CallCustomEvent()
    {
        OnDialogueEnded.Invoke();
    }
}
