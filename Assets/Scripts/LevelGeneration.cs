using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour
{
    [Tooltip("Rooms Settings")]
    [SerializeField] List<GameObject> roomPrefabs = new List<GameObject>();
    [SerializeField] List<int> roomLengths = new List<int>();

    Vector3 currentOffset = Vector3.zero;

    [SerializeField] Transform levelParent;
    [SerializeField] List<GameObject> generatedLevel = new List<GameObject>();
    System.Random random = new System.Random();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateLevel()
    {
        for (int i = 0; i < 10; i++)
        {
            int roomIndex = random.Next(0, roomPrefabs.Count);

            Vector3 spawnPosition = levelParent.position + currentOffset;
            GameObject room = Instantiate(roomPrefabs[roomIndex], spawnPosition, Quaternion.Euler(0f, 90f, 0f), levelParent);
            generatedLevel.Add(room);

            currentOffset += new Vector3(roomLengths[roomIndex], 0f, 0f);
        }
    }
}
