using UnityEngine;

/// <summary>
/// Attach to a large trigger Collider positioned below the entire level.
/// Anything with a PlayerRespawn component gets teleported back to their
/// last checkpoint when they fall into it.
/// </summary>
public class FallCatcher : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
        if (playerRespawn != null)
        {
            playerRespawn.RespawnAtLastCheckpoint();
        }
    }
}
