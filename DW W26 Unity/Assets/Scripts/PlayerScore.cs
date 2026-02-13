using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int score;

    public void Add(int amount)
    {
        score += amount;
        Debug.Log($"{name} Score: {score}");
    }
}
