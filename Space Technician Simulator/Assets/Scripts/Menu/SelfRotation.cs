using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public Vector3 rotationSpeeds = new Vector3(30f, 45f, 60f); // Vitesse pour X, Y, Z

    void Update()
    {
        // Appliquer des rotations indépendantes sur chaque axe
        transform.Rotate(rotationSpeeds * Time.deltaTime);
    }
}
