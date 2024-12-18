using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private float _DetectionRadius = 3f; // The radius to detect the player

    private SphereCollider _SphereCollider;
    private NpcDialogue _NpcDialogue;
    //private NPCBehavior _NpcDialogue;
    [SerializeField] private GameObject _Player;

    public bool IsManualDialogueActive;

    private void Awake()
    {
        _SphereCollider = gameObject.GetComponent<SphereCollider>();
        _SphereCollider.radius = _DetectionRadius;
        _SphereCollider.isTrigger = true;
        IsManualDialogueActive = false;
        _NpcDialogue = null;
    }

    private void Update()
    {
        // Check enter key input
        if (Input.GetKeyDown(KeyCode.Return) && _NpcDialogue != null && !IsManualDialogueActive)
        {
            Debug.Log("Return pressed ! Start Dialogue");

            _NpcDialogue.StartDialogue();
            _SphereCollider.radius = _DetectionRadius + 1;
            IsManualDialogueActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && _NpcDialogue != null && IsManualDialogueActive)
        {
            Debug.Log("NextDialogue");

            // Continue the dialogue
            _NpcDialogue.NextMessageDialogue();
        }
        LookAtNpc();
    }

    public void EndDialogue()
    {
        if (IsManualDialogueActive)
        {
            _SphereCollider.radius = _DetectionRadius;
            IsManualDialogueActive = false;
        }
    }

    public void LookAtNpc()
    {
        if (_NpcDialogue == null || !IsManualDialogueActive) return;


        // Rotate the NPC to face the player
        Vector3 direction = _NpcDialogue.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction);
        _Player.gameObject.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // _Player.gameObject.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player is already in dialogue with a NPC, return
        if (_NpcDialogue != null) return;

        if (other.CompareTag("ManualDialogue"))
        {
            Debug.Log("ManualDialogue Detected Store Object");
            _NpcDialogue = other.GetComponent<NpcDialogue>();
            //_NpcDialogue = other.GetComponent<NPCBehavior>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_NpcDialogue == null) return;

        if (other.CompareTag("ManualDialogue"))
        {
            Debug.Log("ManualDialogue Exit");
            _NpcDialogue = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _DetectionRadius);
    }
}
