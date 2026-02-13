using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int maxLives = 2;
    public int Lives { get; private set; }

    private void Awake()
    {
        Lives = maxLives;
    }

    public bool IsDead => Lives <= 0;

    public void LoseLife(int amount = 1)
    {
        Lives = Mathf.Max(0, Lives - amount);
        Debug.Log($"{name} lives: {Lives}/{maxLives}");
    }
}
