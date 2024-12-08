using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    [SerializeField] public int ActorIndex;
    [SerializeField] private Camera _DialogueCamera;

    // private Transform lastTransform;
    
    private void Start()
    {
        _DialogueCamera.gameObject.SetActive(false);
        _DialogueCamera.enabled = false;
    }

    public void EnableDialogueCamera(Transform sourcePosition)
    {
        _DialogueCamera.gameObject.SetActive(true);
        _DialogueCamera.enabled = true;
        _DialogueCamera.transform.position = new Vector3(sourcePosition.position.x, 2.0f, sourcePosition.position.z);
        Transform lookAt = transform;
        lookAt.position = new Vector3(lookAt.position.x, 1.7f, lookAt.position.z);
        _DialogueCamera.transform.LookAt(lookAt);
    }

    public void DisableDialogueCamera()
    {
        _DialogueCamera.gameObject.SetActive(false);
        _DialogueCamera.enabled = false;
    }
}


    
