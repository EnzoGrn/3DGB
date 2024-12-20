using UnityEngine;

public class NPCInteractionHandler : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private Quest currentQuest;
    private bool IsInteracting = false;

    // THE CODE BELOW IS NECESSARY FOR THE WAYPOINT SYSTEM
    [SerializeField] private GameObject waypoint;
    public GameObject waypointPrefab;
    private GameObject WaypointParent;
    /////////////////////////////////////////////////

    private void Start()
    {
        // THE CODE BELOW IS NECESSARY FOR THE WAYPOINT SYSTEM
        WaypointParent = GameObject.FindGameObjectWithTag("WaypointsParent");
        if (WaypointParent && waypointPrefab)
        {
            waypoint = Instantiate(waypointPrefab, WaypointParent.transform);

            Waypoint waypointScript = GetComponent<Waypoint>();
            if (waypointScript)
            {
                Debug.Log("Setting waypoint UI");
                waypointScript.SetWaypointUI(waypoint);
            }
        }
        /////////////////////////////////////////////////////
    }

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
        IsInteracting = true;
        // Tu peux afficher un UI ici.
    }

    private void HideInteractionPrompt()
    {
        Debug.Log("Interaction ended");
        IsInteracting = false;
        // Cache l'UI ici.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentQuest != null && IsInteracting)
        {
            CompleteInteraction();
        }
    }

    private void CompleteInteraction()
    {
        Debug.Log($"You talked to {npcName}!");
        currentQuest.AdvanceStep();
        // THE CODE BELOW IS NECESSARY FOR THE WAYPOINT SYSTEM
        Waypoint waypointScript = GetComponent<Waypoint>();
        if (waypointScript)
        {
            Debug.Log("Destroying waypoint");
            waypointScript.DestroyWaypoint();
        }
        /////////////////////////////////////////////////////
        HideInteractionPrompt();
    }
}
