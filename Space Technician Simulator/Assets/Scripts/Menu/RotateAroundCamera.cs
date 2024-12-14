using UnityEngine;

public class RotateAroundCamera : MonoBehaviour
{
    public Transform cameraTransform; // Référence vers la caméra
    public float rotationSpeed = 50f; // Vitesse de rotation

    void Update()
    {
        // Faire tourner l'objet autour de la caméra
        transform.RotateAround(cameraTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
