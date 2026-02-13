using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;   // bunnies die after 3 hits
    [SerializeField] private int currentLives;

    public bool IsDead { get; private set; }

    private PlayerRole role;
    private PlayerController controller;

    private void Awake()
    {
        role = GetComponent<PlayerRole>();
        controller = GetComponent<PlayerController>();
        currentLives = maxLives;
    }

    // Call this when fox hits bunny
    public void LoseLife(int amount = 1)
    {
        // Only rabbits have lives (fox shouldn't "die")
        if (role == null || role.role != PlayerRole.Role.Rabbit) return;
        if (IsDead) return;

        currentLives -= amount;

        if (currentLives <= 0)
        {
            IsDead = true;

            // Stop rabbit movement
            if (controller != null) controller.enabled = false;

            // Tell GameManager to check win
            if (GameManager.Instance != null)
                GameManager.Instance.CheckFoxWin();

            Debug.Log("Rabbit is OUT (0 lives).");
            return;
        }

        // Not dead yet -> stun + respawn
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StunThenRespawnRabbit(gameObject, 1f);
        }
        else
        {
            Debug.LogError("GameManager.Instance is NULL (PlayerLives). Add GameManager to gameplay scene + DontDestroyOnLoad.");
        }

        Debug.Log($"Rabbit hit! Lives left: {currentLives}");
    }
}
