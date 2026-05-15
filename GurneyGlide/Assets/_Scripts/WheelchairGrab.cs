using TMPro; // Add this at the top!
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

    private bool isAttached = false;
    private Rigidbody rb;
    private FixedJoint joint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float dist = Vector3.Distance(transform.position, playerTransform.position);
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
            Debug.Log("PATIENT DIED - GAME OVER");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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