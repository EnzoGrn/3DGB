using UnityEngine;

public class Minimap : MonoBehaviour {

    [Header("References")]
    public BoxCollider MapReference;
    public Camera      PlayerCamera;
    public Transform   PlayerTransform;

    [Header("References - UI")]
    public RectTransform MapContainer;
    public RectTransform PlayerIndicator;

    [Header("Parameters")]
    public Vector2 MapTextureSize = new Vector2(1024, 1024);
    public Bounds MapBounds;

    [Header("Player Options")]
    public float MinimapScale = 1f;

    private void Awake()
    {
        if (MapReference) {
            MapReference.gameObject.SetActive(true);

            MapBounds = MapReference.bounds;

            MapReference.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Assign Reference
        Transform rotationReference = PlayerCamera.transform;
        Transform positionReference = PlayerTransform;

        // Calculate unit scale and position offset
        Vector2 unitScale      = new Vector2(MapTextureSize.x / MapBounds.size.x, MapTextureSize.y / MapBounds.size.z);
        Vector3 positionOffset = MapBounds.center - positionReference.position;

        // Assign values
        Vector2    mapPosition    = new Vector2(positionOffset.x * unitScale.x, positionOffset.z * unitScale.y) * MinimapScale;
        Quaternion mapRotation    = default;
        Vector3    mapScale       = new Vector3(MinimapScale, MinimapScale, MinimapScale);
        Quaternion playerRotation = Quaternion.Euler(0, 0, -rotationReference.eulerAngles.y);

        // Set values
        PlayerIndicator.rotation = playerRotation;

        MapContainer.localPosition = mapPosition;
        MapContainer.rotation      = mapRotation;
        MapContainer.localScale    = mapScale;
    }
}
