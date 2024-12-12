using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    // Parent GameObject contenant l'icône et le texte
    private GameObject waypointUI;
    // Icone du waypoint
    private Image img;
    // La cible (ennemi, objet, etc.)
    public Transform target;
    // TextMeshPro pour afficher la distance
    private TextMeshProUGUI meter;
    // Pour ajuster la position de l'icône et du texte
    public Vector3 offset;

    private GameObject player; // Référence au joueur

    private Camera mainCamera;

    [SerializeField] public bool isDisabled;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = player.GetComponentInChildren<Camera>();
        if (player == null)
        {
            Debug.LogError("Player not found");
        }
        ChangeView changeView = player.GetComponent<ChangeView>();
        if (changeView != null)
        {
            changeView.OnCameraChanged += SetCamera;
        }
        isDisabled = false;
    }

    private void SetCamera()
    {
        mainCamera = player.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (target == null || waypointUI == null) return;
        // Calculer la position à l'écran de la cible
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.position + offset);

        // Vérifier si la cible est visible dans le champ de vision
        bool isTargetVisible = IsTargetVisible(targetScreenPos);

        // Montrer ou cacher l'objet parent contenant l'icône et le texte
        waypointUI.SetActive(isTargetVisible && !isDisabled);

        if (isTargetVisible)
        {
            // Limites de la position du waypointUI pour qu'il reste à l'écran
            PositionWaypointUI(targetScreenPos);

            // Calculer la distance entre le joueur et la cible, et mettre à jour le texte
            float distance = Vector3.Distance(player.transform.position, target.position);
            meter.text = ((int)distance).ToString() + "m"; // Afficher la distance
        }
    }

    // Vérifier si la cible est visible dans le champ de vision
    private bool IsTargetVisible(Vector3 targetScreenPos)
    {
        // Si la cible est derrière la caméra, elle n'est pas visible
        if (targetScreenPos.z < 0) return false;

        // Si la cible est en dehors de l'écran, elle n'est pas visible
        if (targetScreenPos.x < 0 || targetScreenPos.x > Screen.width || targetScreenPos.y < 0 || targetScreenPos.y > Screen.height)
        {
            return false;
        }

        return true;
    }

    // Positionner l'objet UI entier (icône + texte)
    private void PositionWaypointUI(Vector3 targetScreenPos)
    {
        // Limiter la position de l'UI du waypoint pour qu'il reste à l'écran
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        // Si la cible est derrière le joueur, placer le waypoint UI de l'autre côté
        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            targetScreenPos.x = (targetScreenPos.x < Screen.width / 2) ? maxX : minX;
        }

        // Limiter la position de l'UI waypoint pour éviter qu'il ne sorte de l'écran
        targetScreenPos.x = Mathf.Clamp(targetScreenPos.x, minX, maxX);
        targetScreenPos.y = Mathf.Clamp(targetScreenPos.y, minY, maxY);

        // Déplacer l'ensemble de l'objet UI à la position calculée
        waypointUI.transform.position = targetScreenPos;
    }

    public void SetWaypointUI(GameObject waypointUI)
    {
        this.waypointUI = waypointUI;
        // Get the Image component in children (the first child)
        img = waypointUI.GetComponentInChildren<Image>();
        meter = waypointUI.GetComponentInChildren<TextMeshProUGUI>();
        waypointUI.SetActive(false);
    }

    public void DestroyWaypoint()
    {
        isDisabled = true;
        if (waypointUI != null)
        {
            Destroy(waypointUI);
        }
        else
        {
            Debug.LogWarning("No waypoint to destroy");
        }
    }
}
