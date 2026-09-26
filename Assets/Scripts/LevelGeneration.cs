using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// CF- changed generate level to public so it can be called  by the level manager to create defined levels for it to access
/// Roman - GenerateLevel now takes a cookie target so the level never spawns more cookies than the level's goal.
/// </summary>
public class LevelGeneration : MonoBehaviour
{
    [Tooltip("Rooms Settings")]
    [SerializeField] List<GameObject> _roomPrefabs = new List<GameObject>();
    [SerializeField] List<int> _roomLengths = new List<int>();

    [Tooltip("Maximum number of rooms to spawn for a level")]
    [SerializeField] int _roomCount = 10;

    Vector3 currentOffset = Vector3.zero;

    [SerializeField] Transform _levelParent;
    [SerializeField] List<GameObject> _generatedLevel = new List<GameObject>();
    System.Random _random = new System.Random();

    public int TotalCookiesSpawned { get; private set; }

    /// <summary>
    /// Generates the level's rooms and spawns cookies, never exceeding cookieTarget in total.
    /// </summary>
    public void GenerateLevel(int cookieTarget)
    {
        currentOffset = Vector3.zero;
        _generatedLevel.Clear();
        TotalCookiesSpawned = 0;

        for (int i = 0; i < _roomCount; i++)
        {
            int roomIndex = _random.Next(0, _roomPrefabs.Count);

            Vector3 spawnPosition = _levelParent.position + currentOffset;
            GameObject room = Instantiate(_roomPrefabs[roomIndex], spawnPosition, Quaternion.Euler(0f, 90f, 0f), _levelParent);
            _generatedLevel.Add(room);

            if (room.TryGetComponent<RoomCookieSpawner>(out var cookieSpawner))
            {
                int remainingBudget = cookieTarget - TotalCookiesSpawned;
                if (remainingBudget > 0)
                {
                    int spawned = cookieSpawner.SpawnCookies(_random, remainingBudget);
                    TotalCookiesSpawned += spawned;
                }
            }

            currentOffset += new Vector3(_roomLengths[roomIndex], 0f, 0f);

            // Stop early once the level's cookie goal has been fully spawned.
            if (TotalCookiesSpawned >= cookieTarget)
                break;
        }
    }
}