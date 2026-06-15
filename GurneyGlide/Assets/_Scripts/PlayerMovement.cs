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
    private bool isCursorLocked = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true; 

        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }

        if (!isCursorLocked && Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                LockCursor();
            }
        }

        if (isCursorLocked)
        {
            yRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }

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
        
        if (isCursorLocked)
        {
            rb.MoveRotation(Quaternion.Euler(0f, yRotation, 0f));
        }
    }

    void LateUpdate()
    {
        if (isCursorLocked)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }
}