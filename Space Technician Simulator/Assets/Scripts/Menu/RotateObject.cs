using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }

    [Header("Rotation Settings")]
    public RotationAxis rotationAxis = RotationAxis.X; // Choix de l'axe de rotation
    public float rotationSpeed = 50f; // Vitesse de rotation (degrés par seconde)

    void Update()
    {
        Vector3 rotationVector = Vector3.zero;

        // Définir le vecteur de rotation en fonction de l'axe choisi
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = new Vector3(1, 0, 0);
                break;
            case RotationAxis.Y:
                rotationVector = new Vector3(0, 1, 0);
                break;
            case RotationAxis.Z:
                rotationVector = new Vector3(0, 0, 1);
                break;
            case RotationAxis.XY:
                rotationVector = new Vector3(1, 1, 0);
                break;
            case RotationAxis.XZ:
                rotationVector = new Vector3(1, 0, 1);
                break;
            case RotationAxis.YZ:
                rotationVector = new Vector3(0, 1, 1);
                break;
            case RotationAxis.XYZ:
                rotationVector = new Vector3(1, 1, 1);
                break;
        }

        // Appliquer la rotation
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }
}
