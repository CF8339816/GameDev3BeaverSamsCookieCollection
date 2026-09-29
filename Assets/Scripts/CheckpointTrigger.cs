using UnityEngine;

/// <summary>
/// Attach to a trigger Collider placed before each room in your level prefabs.
/// When the player passes through, it becomes their new respawn point.
/// </summary>
public class CheckpointTrigger : MonoBehaviour
{
    [Tooltip("Optional explicit respawn point; if left empty, uses this trigger's own position")]
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
        if (playerRespawn == null) return;

        Vector3 point = respawnPoint != null ? respawnPoint.position : transform.position;
        playerRespawn.SetCheckpoint(point);
    }
}
