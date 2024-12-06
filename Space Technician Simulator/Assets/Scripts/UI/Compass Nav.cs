using UnityEngine;

public class CompassNav : MonoBehaviour {

    public Transform ViewDirection;
    public RectTransform CompassElement;
    public float CompassSize;

    private void LateUpdate()
    {
        Vector3 forwardVector = Vector3.ProjectOnPlane(ViewDirection.forward, Vector3.up).normalized;
        
        float forwardSignedAngle = Vector3.SignedAngle(forwardVector, Vector3.forward, Vector3.up);
        float compassOffset      = (forwardSignedAngle / 180.0f) * CompassSize;

        CompassElement.anchoredPosition = new Vector3(compassOffset, 0);
    }
}
