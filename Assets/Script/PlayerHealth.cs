using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 5;
    public int currentHearts;

    public Transform respawnPoint;
    public GameObject[] heartIcons; // Optional: assign your UI heart images

    public bool isHidden = false; // ✅ Added: tracks if player is hiding (e.g., in boat or shelter)

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
        Debug.Log("PlayerHealth initialized with " + currentHearts + " hearts.");
    }

    private void Update()
    {
        // Debug test: Press T key to simulate damage
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test damage input triggered.");
            TakeDamage(1);
        }
    }

    // PUBLIC method - called by other scripts like EarthquakeManager or WaterHazard
    public void TakeDamage(int amount)
    {
        if (currentHearts <= 0)
        {
            Debug.Log("TakeDamage called but player is already at 0 HP.");
            return;
        }

        Debug.Log("TakeDamage called! Damage: " + amount);

        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts); // Prevent negative values

        UpdateHeartsUI();

        if (currentHearts > 0)
        {
            Debug.Log("Player still alive with " + currentHearts + " hearts. Respawning...");
            Respawn();
        }
        else
        {
            Debug.Log("Player died.");
            Die();
        }
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Respawn point not assigned!");
        }

        // Reset physics state to avoid weird momentum on respawn
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Die()
    {
        // Show Game Over screen if GameOverManager is set
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("GameOverManager.Instance is null!");
        }

        // Optional: Disable player GameObject on death
        gameObject.SetActive(false);
    }

    void UpdateHeartsUI()
    {
        if (heartIcons == null || heartIcons.Length == 0) return;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentHearts);
        }

        Debug.Log("Hearts UI updated. Current HP: " + currentHearts);
    }
}
