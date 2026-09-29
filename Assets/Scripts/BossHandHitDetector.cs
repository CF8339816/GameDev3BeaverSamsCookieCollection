using UnityEngine;

/// <summary>
/// Attach to each boss hand GameObject. The hand needs a Collider with "Is Trigger" checked.
/// Detects real physical overlap with the player during an attack swing.
/// </summary>
public class BossHandHitDetector : MonoBehaviour
{
    [SerializeField] private BossController bossController;
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {
        if (bossController == null)
            bossController = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"BossHandHitDetector: OnTriggerEnter with '{other.gameObject.name}', tag='{other.tag}'");

        if (other.CompareTag(playerTag) && bossController != null)
        {
            Debug.Log("BossHandHitDetector: Player hit registered!");
            bossController.RegisterHandHit();
        }
    }
}
