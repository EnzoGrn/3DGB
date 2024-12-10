using System;
using UnityEngine;

public class Quest
{
    public QuestInfoSO QuestInfo { get; private set; }
    public int CurrentStepIndex { get; private set; } = 0;
    public bool IsCompleted { get; private set; } = false;

    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest, int> OnStepUpdated;
    public string DisplayName => QuestInfo.displayName;

    public Quest(QuestInfoSO questInfo)
    {
        QuestInfo = questInfo;
    }

    public void AdvanceStep()
    {
        if (IsCompleted) return;

        CurrentStepIndex++;
        if (CurrentStepIndex >= QuestInfo.questStepPrefabs.Length)
        {
            CompleteQuest();
        }
        else
        {
            OnStepUpdated?.Invoke(this, CurrentStepIndex);
        }
    }

    private void CompleteQuest()
    {
        IsCompleted = true;
        OnQuestCompleted?.Invoke(this);
    }
}
