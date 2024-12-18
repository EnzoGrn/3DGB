using UnityEngine;

public class RotateAroundCamera : MonoBehaviour
{
    public Transform cameraTransform; // R�f�rence vers la cam�ra
    public float rotationSpeed = 50f; // Vitesse de rotation

    void Update()
    {
        // Faire tourner l'objet autour de la cam�ra
        transform.RotateAround(cameraTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
