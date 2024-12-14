using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 10f; // Vitesse de rotation

    void Update()
    {
        // Modifier la rotation de la skybox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
