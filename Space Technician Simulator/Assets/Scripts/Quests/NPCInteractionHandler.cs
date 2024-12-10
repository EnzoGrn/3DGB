using UnityEngine;

public class NPCInteractionHandler : MonoBehaviour
{
    [SerializeField] private string npcName;
    private Quest currentQuest;

    public void Initialize(Quest quest)
    {
        currentQuest = quest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowInteractionPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideInteractionPrompt();
        }
    }

    private void ShowInteractionPrompt()
    {
        Debug.Log($"Press 'E' to talk to {npcName}");
        // Tu peux afficher un UI ici.
    }

    private void HideInteractionPrompt()
    {
        Debug.Log("Interaction ended");
        // Cache l'UI ici.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentQuest != null)
        {
            CompleteInteraction();
        }
    }

    private void CompleteInteraction()
    {
        Debug.Log($"You talked to {npcName}!");
        currentQuest.AdvanceStep();
        HideInteractionPrompt();

        // Optionnel : Ajoute un système de dialogue ou d’autres actions ici.
    }
}
