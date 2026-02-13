using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxCarrots = 2;
    public int carriedCarrots;

    // Slow amounts (tweak in inspector)
    [Header("Movement Slow (Bunny only)")]
    [Range(0f, 1f)] public float slowMultiplierFor1 = 0.75f; // 25% slower
    [Range(0f, 1f)] public float slowMultiplierFor2 = 0.50f; // 50% slower

    public bool CanPickup(int amount) => carriedCarrots + amount <= maxCarrots;

    public void AddCarrot(int amount)
    {
        carriedCarrots = Mathf.Clamp(carriedCarrots + amount, 0, maxCarrots);
        Debug.Log($"{name} carried carrots: {carriedCarrots}");
    }

    public int DepositAll()
    {
        int amount = carriedCarrots;
        carriedCarrots = 0;
        return amount;
    }

    // PlayerController will call this every frame
    public float GetMoveSpeedMultiplier()
    {
        if (carriedCarrots <= 0) return 1f;
        if (carriedCarrots == 1) return slowMultiplierFor1;
        return slowMultiplierFor2; // 2 or more (clamped anyway)
    }
}
