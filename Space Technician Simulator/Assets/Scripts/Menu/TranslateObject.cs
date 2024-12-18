using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    public enum TranslationAxis
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }

    [Header("Translation Settings")]
    public TranslationAxis translationAxis = TranslationAxis.X; // Choix de l'axe de translation
    public float translationSpeed = 5f; // Vitesse de translation (unités par seconde)

    [Header("Position Limits")]
    public Vector3 startPosition = Vector3.zero; // Coordonnées de départ
    public Vector3 positionLimits = new Vector3(10f, 10f, 10f); // Limites maximales de translation

    void Start()
    {
        // Initialiser la position de départ
        transform.position = startPosition;
        Debug.Log("Start position: " + startPosition);
        Debug.Log("Position limits: " + positionLimits);
    }

    void Update()
    {
        Vector3 translationVector = Vector3.zero;

        // Définir le vecteur de translation en fonction de l'axe choisi
        switch (translationAxis)
        {
            case TranslationAxis.X:
                translationVector = new Vector3(1, 0, 0);
                break;
            case TranslationAxis.Y:
                translationVector = new Vector3(0, 1, 0);
                break;
            case TranslationAxis.Z:
                translationVector = new Vector3(0, 0, 1);
                break;
            case TranslationAxis.XY:
                translationVector = new Vector3(1, 1, 0);
                break;
            case TranslationAxis.XZ:
                translationVector = new Vector3(1, 0, 1);
                break;
            case TranslationAxis.YZ:
                translationVector = new Vector3(0, 1, 1);
                break;
            case TranslationAxis.XYZ:
                translationVector = new Vector3(1, 1, 1);
                break;
        }

        // Appliquer la translation
        transform.Translate(translationVector * translationSpeed * Time.deltaTime);

        // Vérifier les limites et réinitialiser si nécessaire
        Vector3 pos = transform.position;
        switch (translationAxis)
        {
            case TranslationAxis.X:
                if (pos.x > positionLimits.x)
                {
                    transform.position = startPosition;
                }
                break;
            case TranslationAxis.Y:
                if (pos.y > positionLimits.y)
                {
                    transform.position = startPosition;
                }
                break;
            case TranslationAxis.Z:
                if (pos.z > positionLimits.z)
                {
                    transform.position = startPosition;
                }
                break;
            case TranslationAxis.XY:
                if (pos.x < startPosition.x || pos.x > startPosition.x + positionLimits.x)
                {
                    pos.x = Mathf.Clamp(pos.x, startPosition.x, startPosition.x + positionLimits.x);
                    transform.position = pos;
                }
                if (pos.y < startPosition.y || pos.y > startPosition.y + positionLimits.y)
                {
                    pos.y = Mathf.Clamp(pos.y, startPosition.y, startPosition.y + positionLimits.y);
                    transform.position = pos;
                }
                break;
            case TranslationAxis.XZ:
                if (pos.x < startPosition.x || pos.x > startPosition.x + positionLimits.x)
                {
                    pos.x = Mathf.Clamp(pos.x, startPosition.x, startPosition.x + positionLimits.x);
                    transform.position = pos;
                }
                if (pos.z < startPosition.z || pos.z > startPosition.z + positionLimits.z)
                {
                    pos.z = Mathf.Clamp(pos.z, startPosition.z, startPosition.z + positionLimits.z);
                    transform.position = pos;
                }
                break;
            case TranslationAxis.YZ:
                if (pos.y < startPosition.y || pos.y > startPosition.y + positionLimits.y)
                {
                    pos.y = Mathf.Clamp(pos.y, startPosition.y, startPosition.y + positionLimits.y);
                    transform.position = pos;
                }
                if (pos.z < startPosition.z || pos.z > startPosition.z + positionLimits.z)
                {
                    pos.z = Mathf.Clamp(pos.z, startPosition.z, startPosition.z + positionLimits.z);
                    transform.position = pos;
                }
                break;
            case TranslationAxis.XYZ:
                if (pos.x < startPosition.x || pos.x > startPosition.x + positionLimits.x)
                {
                    pos.x = Mathf.Clamp(pos.x, startPosition.x, startPosition.x + positionLimits.x);
                    transform.position = pos;
                }
                if (pos.y < startPosition.y || pos.y > startPosition.y + positionLimits.y)
                {
                    pos.y = Mathf.Clamp(pos.y, startPosition.y, startPosition.y + positionLimits.y);
                    transform.position = pos;
                }
                if (pos.z < startPosition.z || pos.z > startPosition.z + positionLimits.z)
                {
                    pos.z = Mathf.Clamp(pos.z, startPosition.z, startPosition.z + positionLimits.z);
                    transform.position = pos;
                }
                break;
        }
    }
}
