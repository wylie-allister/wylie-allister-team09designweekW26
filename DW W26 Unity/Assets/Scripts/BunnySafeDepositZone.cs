using UnityEngine;

public class BunnySafeDepositZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var dep = other.GetComponent<PlayerDeposit>();
        if (dep != null) dep.SetInSafeZone(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var dep = other.GetComponent<PlayerDeposit>();
        if (dep != null) dep.SetInSafeZone(false);
    }
}
