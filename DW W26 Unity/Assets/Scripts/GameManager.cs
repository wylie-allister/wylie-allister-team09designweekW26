using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Winner { None, Bunnies, Foxes }
    public static Winner LastWinner = Winner.None;

    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "Dual Monitor Scene"; // set to your gameplay scene name
    [SerializeField] private string endSceneName = "EndScene";               // set to your end scene name

    public static GameManager Instance { get; private set; }

    [Header("Carrot Win Condition")]
    [SerializeField] private int totalCarrotsInLevel;
    [SerializeField] private int depositedCarrots;

    [Header("Fox Win Condition")]
    [SerializeField] private int rabbitsOutCount;

    [Header("Rabbit Respawn")]
    [SerializeField] private Transform rabbitRespawnPoint;

    [Header("State")]
    public bool gameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // IMPORTANT: keeps GameManager alive from Team Select -> Gameplay -> EndScene
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only set up match state when gameplay scene loads
        if (scene.name != gameplaySceneName) return;

        gameEnded = false;
        depositedCarrots = 0;
        rabbitsOutCount = 0;
        LastWinner = Winner.None;

        // Count carrots present at the start of the match
        totalCarrotsInLevel = GameObject.FindGameObjectsWithTag("Carrot").Length;

        Debug.Log($"[GameManager] Gameplay loaded. Total carrots in level: {totalCarrotsInLevel}");
    }

    // BUNNY WIN
    public void AddDepositedCarrots(int amount)
    {
        if (gameEnded) return;

        depositedCarrots += amount;
        Debug.Log($"Deposited carrots: {depositedCarrots}/{totalCarrotsInLevel}");

        if (depositedCarrots >= totalCarrotsInLevel && totalCarrotsInLevel > 0)
        {
            BunnyWin();
        }
    }

    private void BunnyWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        LastWinner = Winner.Bunnies;
        Debug.Log("BUNNIES WIN! All carrots deposited. Loading end scene...");

        SceneManager.LoadScene(endSceneName);
    }

    // FOX WIN
    public void CheckFoxWin()
    {
        if (gameEnded) return;

        int outCount = 0;
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            var role = p.GetComponent<PlayerRole>();
            if (role == null || role.role != PlayerRole.Role.Rabbit) continue;

            var lives = p.GetComponent<PlayerLives>();
            if (lives != null && lives.IsDead) outCount++;
        }

        rabbitsOutCount = outCount;

        // If BOTH rabbits are out, foxes win
        if (rabbitsOutCount >= 2)
        {
            FoxWin();
        }
    }

    private void FoxWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        LastWinner = Winner.Foxes;
        Debug.Log("FOXES WIN! Both bunnies eliminated. Loading end scene...");

        SceneManager.LoadScene(endSceneName);
    }

    // RABBIT RESPAWN + STUN
    public void RespawnRabbit(Transform rabbit)
    {
        if (rabbitRespawnPoint == null || rabbit == null) return;

        rabbit.position = rabbitRespawnPoint.position;

        var rb = rabbit.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public void StunThenRespawnRabbit(GameObject rabbit, float stunSeconds = 1f)
    {
        if (rabbit == null) return;
        StartCoroutine(StunRespawnRoutine(rabbit, stunSeconds));
    }

    private System.Collections.IEnumerator StunRespawnRoutine(GameObject rabbit, float stunSeconds)
    {
        // Disable movement during stun
        var controller = rabbit.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        // Stop movement instantly
        var rb = rabbit.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Wait
        yield return new WaitForSeconds(stunSeconds);

        // Respawn at safe point
        RespawnRabbit(rabbit.transform);

        // Re-enable movement (only if not dead)
        var lives = rabbit.GetComponent<PlayerLives>();
        bool isDead = (lives != null && lives.IsDead);

        if (!isDead && controller != null)
            controller.enabled = true;
    }
}
