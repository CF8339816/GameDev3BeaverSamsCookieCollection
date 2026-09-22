using System;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] List<Transform> _jumpPositions = new List<Transform>();
    [SerializeField] List<Transform> _handPrepPosition = new List<Transform>();

    [SerializeField] GameObject _bossHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        Transform _currentTarget = _jumpPositions[UnityEngine.Random.Range(0, _jumpPositions.Count)];
        Transform _prepPos = _handPrepPosition[UnityEngine.Random.Range(0, _handPrepPosition.Count)];

        _bossHand.transform.position = _prepPos.position;
    }
}
