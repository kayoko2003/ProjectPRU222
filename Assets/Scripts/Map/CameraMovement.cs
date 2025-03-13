using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float mapSize = 100f; // Kích thước bản đồ (200x200)
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        Camera cam = Camera.main;
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * cam.aspect;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 newPos = target.position + offset;

        // Tính toán giới hạn chính xác dựa vào kích thước camera
        float minX = -mapSize + cameraHalfWidth;
        float maxX = mapSize - cameraHalfWidth;
        float minY = -mapSize + cameraHalfHeight;
        float maxY = mapSize - cameraHalfHeight;

        // Giới hạn camera
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.z = transform.position.z; // Giữ nguyên trục Z

        transform.position = newPos;
    }
}
