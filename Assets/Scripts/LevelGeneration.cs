using UnityEngine;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour
{
    [Tooltip("Rooms Settings")]
    [SerializeField] List<GameObject> _roomPrefabs = new List<GameObject>();
    [SerializeField] List<int> _roomLengths = new List<int>();

    Vector3 currentOffset = Vector3.zero;

    [SerializeField] Transform _levelParent;
    [SerializeField] List<GameObject> _generatedLevel = new List<GameObject>();
    System.Random _random = new System.Random();

    void Start()
    {
        GenerateLevel();
    }

    void Update()
    {

    }

    void GenerateLevel()
    {
        for (int i = 0; i < 10; i++)
        {
            int roomIndex = _random.Next(0, _roomPrefabs.Count);

            Vector3 spawnPosition = _levelParent.position + currentOffset;
            GameObject room = Instantiate(_roomPrefabs[roomIndex], spawnPosition, Quaternion.Euler(0f, 90f, 0f), _levelParent);
            _generatedLevel.Add(room);

            if (room.TryGetComponent<RoomCookieSpawner>(out var cookieSpawner))
            {
                cookieSpawner.SpawnCookies(_random);
            }

            currentOffset += new Vector3(_roomLengths[roomIndex], 0f, 0f);
        }
    }
}