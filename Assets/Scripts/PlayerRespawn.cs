using UnityEngine;

/// <summary>
/// Attach to the Player GameObject. Tracks the last checkpoint touched
/// and teleports the player back to it when they fall off the level.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Fallback checkpoint: wherever the player starts, in case they fall
        // before touching any checkpoint trigger.
        lastCheckpointPosition = transform.position;
        hasCheckpoint = true;
    }

    /// <summary>
    /// Called by CheckpointTrigger when the player passes through a checkpoint.
    /// </summary>
    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
        Debug.Log($"PlayerRespawn: checkpoint updated to {position}");
    }

    /// <summary>
    /// Called by FallCatcher when the player falls off the level.
    /// </summary>
    public void RespawnAtLastCheckpoint()
    {
        if (!hasCheckpoint) return;

        // CharacterController overrides direct transform changes unless it's
        // temporarily disabled - this is the standard Unity teleport pattern.
        controller.enabled = false;
        transform.position = lastCheckpointPosition;
        controller.enabled = true;

        Debug.Log($"PlayerRespawn: respawned at {lastCheckpointPosition}");
    }
}
