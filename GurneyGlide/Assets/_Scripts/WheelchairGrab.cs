using TMPro; 
using UnityEngine;

public class WheelchairGrab : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 3f;
    public Transform playerTransform; 
    public Vector3 attachOffset = new Vector3(0, -1.1f, 1.3f); 
    
    [Header("Heart Rate System")]
    public TextMeshProUGUI hrDisplay; 
    public float currentHR = 70f;
    public float maxHR = 180f;

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverMessageObject;
    [SerializeField] private GameObject restartButton; 

    [Header("UI Interaction Prompt")]
    [SerializeField] private GameObject interactPromptObject;

    private bool isAttached = false;
    private Rigidbody rb;
    private FixedJoint joint;
    private bool isDead = false; 
    private bool isWon = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (gameOverMessageObject != null) gameOverMessageObject.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (interactPromptObject != null) interactPromptObject.SetActive(false);
    }

    void Update()
    {
        if (isDead || isWon) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (interactPromptObject != null)
        {
            if (!isAttached && dist <= grabRange)
            {
                interactPromptObject.SetActive(true);
            }
            else
            {
                interactPromptObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttached && dist <= grabRange) Grab();
            else if (isAttached) Drop();
        }

        if (currentHR > 70f)
        {
            currentHR -= Time.deltaTime * 2f;
        }

        if (hrDisplay != null)
        {
            hrDisplay.text = "HR: " + Mathf.RoundToInt(currentHR) + " BPM";
            hrDisplay.color = currentHR > 120 ? Color.red : Color.white;
        }

        if (currentHR >= maxHR)
        {
            GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead || isWon) return;

        if (isAttached)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            if (impactForce > 1.5f)
            {
                currentHR += impactForce * 5f;
                Debug.Log("BUMP! HR Spiked!");
            }
        }
    }

    public void GameWon()
    {
        isWon = true;

        if (interactPromptObject != null) 
        {
            interactPromptObject.SetActive(false);
        }

        if (hrDisplay != null)
        {
            hrDisplay.text = "HR: " + Mathf.RoundToInt(currentHR) + " BPM";
        }
    }

    void GameOver()
    {
        isDead = true;
        currentHR = 0f; 

        if (hrDisplay != null)
        {
            hrDisplay.text = "HR: 0 BPM";
            hrDisplay.color = Color.red;
        }

        if (gameOverMessageObject != null)
        {
            gameOverMessageObject.SetActive(true);
            TextMeshProUGUI deathText = gameOverMessageObject.GetComponent<TextMeshProUGUI>();
            if (deathText != null)
            {
                deathText.text = "YOU DIED";
                deathText.color = Color.red;
            }
        }

        if (restartButton != null) restartButton.SetActive(true);
        if (interactPromptObject != null) interactPromptObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isAttached) Drop();

        if (playerTransform != null)
        {
            PlayerMovement playerMove = playerTransform.GetComponent<PlayerMovement>();
            if (playerMove != null) playerMove.enabled = false; 

            Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();
            if (playerRb != null) playerRb.linearVelocity = Vector3.zero;
        }
    }

    void Grab()
    {
        isAttached = true;
        transform.position = playerTransform.TransformPoint(attachOffset);
        transform.rotation = playerTransform.rotation * Quaternion.Euler(0, 180, 0);

        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = playerTransform.GetComponentInParent<Rigidbody>();
        
        gameObject.layer = LayerMask.NameToLayer("Wheelchair");
        rb.useGravity = false; 
    }

    void Drop()
    {
        isAttached = false;
        if (joint != null) Destroy(joint);
        rb.useGravity = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}