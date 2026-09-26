using UnityEngine;
using System.Collections.Generic;

public class RoomCookieSpawner : MonoBehaviour
{
    [Tooltip("All possible points where a cookie can spawn in this room")]
    [SerializeField] List<Transform> _cookieSpawnPoints = new List<Transform>();

    [SerializeField] GameObject _cookiePrefab;

    [Tooltip("Maximum number of cookies in one room")]
    [SerializeField] int _maxCookiesPerRoom = 2;

    /// <summary>
    /// Spawns cookies in this room, capped by both the room's own max
    /// and the remaining budget allowed for the whole level.
    /// Returns how many cookies were actually spawned.
    /// </summary>
    public int SpawnCookies(System.Random random, int maxAllowed)
    {
        if (_cookieSpawnPoints.Count == 0 || _cookiePrefab == null || maxAllowed <= 0)
            return 0;

        // Shuffle the points so that random ones are chosen each time
        List<Transform> shuffled = new List<Transform>(_cookieSpawnPoints);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        int count = Mathf.Min(Mathf.Min(_maxCookiesPerRoom, shuffled.Count), maxAllowed);
        for (int i = 0; i < count; i++)
        {
            Instantiate(_cookiePrefab, shuffled[i].position, Quaternion.identity, transform);
        }

        return count;
    }
}
