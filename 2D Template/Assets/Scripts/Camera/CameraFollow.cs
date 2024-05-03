using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Transform target;
    public float smoothSpeed = 1f;
    public Vector3 offset;

    private void Start() {
        target = PlayerMovement.Instance.transform;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}