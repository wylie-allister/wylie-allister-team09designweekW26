using UnityEngine;

public class BurrowPortal : MonoBehaviour
{
    // Each burrow will have an identity ID (0,1,2,3) Which helps to track which one was used
    [Header("Burrow Identity")]
    public int portalId = 0;

    // Location where the player will be sent too
    [Header("Where This Burrow Will Send Player")]
    public Transform destination;

    [Header("Rules")]
    // If true only rabbits can use this portal
    public bool rabbitsOnly = true;

    // How many seconds before this player can use any burrow again
    public float globalCooldownSeconds = 5f;

    // Prevents players from going back into the same burrow
    public bool blockSamePortal = true;

    // Runs when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If its not tagged "Player" then ignore it
        if (!other.CompareTag("Player")) 
            return;

        // Get the PlayerRole component from whoever touched the portal
        var role = other.GetComponent<PlayerRole>();
        if (role == null) 
            return;

        // If portal only allows rabbits and this is not a rabbit then stop
        if (rabbitsOnly && role.role != PlayerRole.Role.Rabbit) 
            return;

        if (destination == null) return;

        // Get or make the BurrowTravelState component on this player
        var travel = other.GetComponent<BurrowTravelState>();

        if (travel == null) travel = other.gameObject.AddComponent<BurrowTravelState>();

        // cooldown check
        if (!travel.CanUseBurrow(globalCooldownSeconds)) 
            return;

        // same-portal check
        if (blockSamePortal && travel.lastPortalId == portalId) 
            return;

        // Teleport
        other.transform.position = destination.position;

        // Stop carry-over movement
        var rb = other.GetComponent<Rigidbody2D>();

        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Record use
        travel.RecordUse(portalId);
    }
}
