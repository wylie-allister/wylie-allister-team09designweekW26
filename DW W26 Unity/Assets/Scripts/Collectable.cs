using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var role = other.GetComponent<PlayerRole>();
        if (role == null || role.role != PlayerRole.Role.Rabbit) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) inv = other.gameObject.AddComponent<PlayerInventory>();

        // max carry check 2 
        if (!inv.CanPickup(value))
        {
            Debug.Log($"{other.name} inventory full. Can't pick up more.");
            return;
        }

        inv.AddCarrot(value);
        Destroy(gameObject);
    }
}
