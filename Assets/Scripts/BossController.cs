using System;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] List<Transform> _jumpPositions = new List<Transform>();
    [SerializeField] List<Transform> _handPrepPosition = new List<Transform>();
    [SerializeField] Transform _restPosition;

    [SerializeField] GameObject _bossHand;

    [Header("Attack Timer Settings")]
    [SerializeField] float _attackTimer = 0f;
    [SerializeField] float _attackInterval = 5f;
    [SerializeField] float _minAttackInterval = 2f;
    [SerializeField] float _intervalChange = 0.5f;

    [SerializeField] Transform _currentTarget;
    [SerializeField] Transform _previousTarget;
    [SerializeField] Transform _prepPos;

    System.Random rand = new System.Random();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackInterval)
        {
            _attackTimer = 0f;
            Attack();
            if (_attackInterval > _minAttackInterval)
            {
                _attackInterval -= _intervalChange;
            }
        }
    }

    /*void Attack()
    {
        _prepPos = _handPrepPosition[rand.Next(0, _handPrepPosition.Count)];
        _currentTarget = _jumpPositions[rand.Next(0, _jumpPositions.Count)];

        _bossHand.transform.position = _prepPos.position;
    }*/

    void Attack()
    {
        int i;

        Transform previousPrep = _prepPos;

        do
        {
            i = rand.Next(0, _handPrepPosition.Count);
            _prepPos = _handPrepPosition[i];
            _currentTarget = _jumpPositions[i];
        }
        while (_prepPos == previousPrep && _handPrepPosition.Count > 1);

        _previousTarget = previousPrep;

        Debug.Log("Setting boss hand to selected position");
        _bossHand.transform.position = _prepPos.position;
    }
}
