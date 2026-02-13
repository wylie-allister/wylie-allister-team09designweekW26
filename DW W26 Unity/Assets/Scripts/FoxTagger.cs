using UnityEngine;

public class FoxTagger : MonoBehaviour
{
    [Header("Tag Rules")]
    [SerializeField] private float tagCooldown = 1.0f;
    [SerializeField] private float stunSeconds = 1.0f;

    [Header("Carrot Drop")]
    [SerializeField] private GameObject carrotDropPrefab;
    [SerializeField] private float dropScatterRadius = 0.35f; // how far carrots scatter

    private float nextTagTime;
    private PlayerRole myRole;

    private void Awake()
    {
        myRole = GetComponent<PlayerRole>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < nextTagTime) return;
        if (!other.CompareTag("Player")) return;

        // Only fox can tag
        if (myRole == null || myRole.role != PlayerRole.Role.Fox) return;

        // Only rabbits are targets
        var targetRole = other.GetComponent<PlayerRole>();
        if (targetRole == null || targetRole.role != PlayerRole.Role.Rabbit) return;

        if (GameManager.Instance == null || GameManager.Instance.gameEnded) return;

        var lives = other.GetComponent<PlayerLives>();
        if (lives == null) lives = other.gameObject.AddComponent<PlayerLives>();
        if (lives.IsDead) return;

        // Lose a life
        lives.LoseLife(1);
        nextTagTime = Time.time + tagCooldown;

        // Drop carried carrots physically
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            int carried = inv.DepositAll(); // clears inventory + returns amount

            if (carried > 0 && carrotDropPrefab != null)
            {
                for (int i = 0; i < carried; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * dropScatterRadius;
                    Vector3 pos = other.transform.position + (Vector3)offset;

                    Instantiate(carrotDropPrefab, pos, Quaternion.identity);
                }
            }
        }

        // Stun + respawn
        GameManager.Instance.StunThenRespawnRabbit(other.gameObject, stunSeconds);

        // Check fox win
        GameManager.Instance.CheckFoxWin();
    }
}
