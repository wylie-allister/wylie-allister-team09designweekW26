using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeposit : MonoBehaviour
{
    private bool inSafeZone;

    private PlayerRole role;

    private void Awake()
    {
        role = GetComponent<PlayerRole>();
    }

    public void SetInSafeZone(bool value)
    {
        inSafeZone = value;
    }

    // Hook this to an Input Action (e.g., "Deposit" / "Interact")
    public void OnDeposit()
    {
        if (role == null || role.role != PlayerRole.Role.Rabbit) return;
        if (!inSafeZone) return;

        var inv = GetComponent<PlayerInventory>();
        if (inv == null) return;

        int amount = inv.DepositAll();
        if (amount <= 0) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL. Add GameManager to the scene.");
            return;
        }

        GameManager.Instance.AddDepositedCarrots(amount);
        Debug.Log($"Deposited {amount} carrots.");
    }

}

