using UnityEngine;
using TMPro;

public class WinZone : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winMessageObject;
    [SerializeField] private GameObject restartButton; 
    
    [Header("Text Settings")]
    [SerializeField] private string victoryText = "You Escaped!";

    private bool gameHasEnded = false; 

    private void Start()
    {
        if (winMessageObject != null) winMessageObject.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameHasEnded) return;

        if (other.CompareTag("Player") || other.GetComponent<WheelchairGrab>() != null || other.GetComponentInParent<WheelchairGrab>() != null)
        {
            if (winMessageObject != null)
            {
                gameHasEnded = true; 

                float timeTaken = Time.timeSinceLevelLoad;
                int minutes = Mathf.FloorToInt(timeTaken / 60f);
                int seconds = Mathf.FloorToInt(timeTaken % 60f);
                string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

                int score = 50; 
                
                WheelchairGrab wheelchair = FindFirstObjectByType<WheelchairGrab>();
                if (wheelchair != null)
                {
                    wheelchair.GameWon();

                    FixedJoint joint = wheelchair.GetComponent<FixedJoint>();
                    if (joint != null && joint.connectedBody != null)
                    {
                        score = 100; 
                    }
                }

                winMessageObject.SetActive(true);
                
                TextMeshProUGUI textComp = winMessageObject.GetComponent<TextMeshProUGUI>();
                if (textComp != null)
                {
                    textComp.text = victoryText + 
                                   "\nTime: " + formattedTime + 
                                   "\nScore: " + score + " pts";
                }

                if (restartButton != null) restartButton.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PlayerMovement playerMove = other.GetComponent<PlayerMovement>() ?? other.GetComponentInParent<PlayerMovement>();
                
                if (playerMove == null)
                {
                    GameObject playerObj = GameObject.FindWithTag("Player");
                    if (playerObj != null) playerMove = playerObj.GetComponent<PlayerMovement>();
                }

                if (playerMove != null) playerMove.enabled = false;

                Rigidbody playerRb = other.GetComponent<Rigidbody>() ?? other.GetComponentInParent<Rigidbody>();
                if (playerRb == null && playerMove != null) playerRb = playerMove.GetComponent<Rigidbody>();
                
                if (playerRb != null) playerRb.linearVelocity = Vector3.zero;
            }
        }
    }
}