using UnityEngine;

public class QuestStepHandler : MonoBehaviour
{
    public Quest CurrentQuest { get; private set; }

    public void Initialize(Quest quest)
    {
        CurrentQuest = quest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CompleteStep();
        }
    }

    private void CompleteStep()
    {
        CurrentQuest?.AdvanceStep();
        Destroy(gameObject); // Optionnel : détruire l'objet après validation.
    }
}
