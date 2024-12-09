using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    [SerializeField] public int ActorIndex;
    [SerializeField] public Camera Camera;

    // private Transform lastTransform;
    
    private void Start()
    {
        Camera.gameObject.SetActive(false);
        Camera.enabled = false;
    }

    public void EnableDialogueCamera(Transform sourcePosition)
    {
        Camera.gameObject.SetActive(true);
        Camera.enabled = true;

        // Set the camera position above the other object
        // Convert Camera position to the parent gameObjects local space

        // Set the camera position above the source object

        Camera.transform.position = new Vector3(sourcePosition.position.x, 1.5f, sourcePosition.position.z);

        // Look at the trnasform of actual game object + height of the objetc to see the head
        Camera.transform.LookAt(transform.position + Vector3.up);

    }

    public void DisableDialogueCamera()
    {
        Camera.gameObject.SetActive(false);
        Camera.enabled = false;
    }
}


    
