using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<QuestInfoSO> exampleQuests;

    private void Start()
    {
        foreach (QuestInfoSO exampleQuest in exampleQuests)
            QuestManager.Instance.StartQuest(exampleQuest);
    }
}
