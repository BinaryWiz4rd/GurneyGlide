using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 7f;
    public Transform playerCamera;

    private Rigidbody rb;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true; 

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 0.1f) 
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = (transform.right * moveX) + (transform.forward * moveZ);
        if (moveDir.magnitude > 1) moveDir.Normalize();

        float verticalAdjust = rb.linearVelocity.y;
        if (moveDir.magnitude > 0.1f && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            verticalAdjust = 0.1f;
        }

        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, verticalAdjust, moveDir.z * moveSpeed);
        rb.MoveRotation(Quaternion.Euler(0f, yRotation, 0f));
    }

    void LateUpdate()
    {
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}