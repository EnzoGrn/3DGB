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

        // Check the distance between 2 objects, if too close then move the camera to the side to see both players
        float distance = Vector3.Distance(sourcePosition.position, transform.position);

        if (distance < 1.5f)
        {
            float x = transform.position.x + Mathf.Cos(45) * 3f;
            float z = transform.position.z + Mathf.Sin(45) * 3f;
        
            Camera.transform.position = new Vector3(x, 1.7f, z);

            // Look at the middle point between the 2 objects
            Vector3 lookAt = (sourcePosition.position + transform.position) / 2;
            lookAt.y += 1.7f;
            Camera.transform.LookAt(lookAt);
        }
        else
        {
            Camera.transform.position = new Vector3(sourcePosition.position.x, 1.7f, sourcePosition.position.z);
            Vector3 lookAt = transform.position;
            lookAt.y += 1.7f;
            Camera.transform.LookAt(lookAt);
        }
        
        
        // Camera.transform.LookAt(transform.position + Vector3.up);
    }

    public void DisableDialogueCamera()
    {
        Camera.gameObject.SetActive(false);
        Camera.enabled = false;
    }
}


    
