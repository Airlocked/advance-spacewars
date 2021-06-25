using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls camera on a battlefield
/// </summary>
public partial class CameraActor : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] float moveSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [Space]
    [SerializeField] float rotationSpeed;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;

    [Header("Debug")]
    [SerializeField] float currentDistance;
    [SerializeField] Vector3 currentPosition;
    [SerializeField] Vector3 currentEuler;

    private Vector3 lastMousePosition;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class CameraActor
{
    private void Awake()
    {
        if (maxDistance < minDistance) maxDistance = minDistance;
        if (maxAngle < minAngle) maxAngle = minAngle;

        currentDistance = minDistance + (maxDistance - minDistance) / 2f;
        currentEuler = new Vector3
        {
            x = maxAngle,
            y = 0,
            z = 0
        };
    }

    private void Update()
    {
        HandleRotationInput();
        HandlePositionInput();
        HandleZoomInput();
        transform.rotation = Quaternion.Euler(currentEuler);
        transform.position = currentPosition + transform.forward * currentDistance * -1;
    }
}

/// <summary>
/// Public Actions
/// </summary>
public partial class CameraActor
{

}

/// <summary>
/// Private Actions
/// </summary>
public partial class CameraActor
{
    private void HandleRotationInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            var mouseDeltaRaw = Input.mousePosition - lastMousePosition;
            var mouseDelta = new Vector2
            {
                x = mouseDeltaRaw.x / Screen.width,
                y = mouseDeltaRaw.y / Screen.height
            };

            currentEuler = new Vector3
            {
                x = Mathf.Clamp(currentEuler.x - mouseDelta.y * rotationSpeed, minAngle, maxAngle), // Angle
                y = currentEuler.y + mouseDelta.x * rotationSpeed, // Rotation
                z = currentEuler.z
            };

            lastMousePosition = Input.mousePosition;
        }
    }

    private void HandlePositionInput()
    {
        var forward = new Vector3
        {
            x = transform.forward.x,
            y = 0,
            z = transform.forward.z
        }.normalized;

        var right = new Vector3
        {
            x = transform.right.x,
            y = 0,
            z = transform.right.z
        }.normalized;

        var delta = forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");

        currentPosition += delta * moveSpeed * Time.deltaTime;
    }

    private void HandleZoomInput()
    {
        var delta = Input.mouseScrollDelta.y * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance + delta, minDistance, maxDistance);
    }
}

/// <summary>
/// Gizmos
/// </summary>
public partial class CameraActor
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, currentPosition);
    }
}
