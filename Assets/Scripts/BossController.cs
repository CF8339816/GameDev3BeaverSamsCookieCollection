using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// CF- added a proxy is attacking bool value that will alow the event manager to check the bool status externally wiuthout the ability to change it 
/// </summary>

public class BossController : MonoBehaviour
{
    [SerializeField] List<Transform> _jumpPositions = new List<Transform>();
    [SerializeField] List<Transform> _handPrepPosition = new List<Transform>();
    [SerializeField] Transform _restPosition;

    [SerializeField] GameObject _bossHandLeft;
    [SerializeField] GameObject _bossHandRight;
    [SerializeField] Transform _leftRestPosition;
    [SerializeField] Transform _rightRestPosition;

    // indices into _handPrepPosition / _jumpPositions that each hand is allowed to use
    static readonly int[] LeftAllowedIndices = { 0, 1 };
    static readonly int[] RightAllowedIndices = { 1, 2 };

    const int CenterIndex = 1;
    const float CenterLineChance = 0.25f;

    [Header("Attack Timer Settings")]
    [SerializeField] float _attackTimer = 0f;
    [SerializeField] float _attackInterval = 5f;
    [SerializeField] float _minAttackInterval = 2f;
    [SerializeField] float _intervalChange = 0.5f;

    int _previousLeftIndex = -1;
    int _previousRightIndex = -1;

    [Header("Hand Movement Settings")]
    [SerializeField] float _handMoveSpeed;

    [Header("Wind-Up Settings")]
    [SerializeField] float _windUpPullbackDistance = 0.5f;
    [SerializeField] float _windUpPauseDuration = 0.5f;

    bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;  /// CF added this to give other scripts something to check against that they cannot change

    public static event Action<GameObject, Vector3> OnHandMovementStarted; /// CF added this to give other scripts something to check against 



    System.Random rand = new System.Random();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_bossHandLeft != null && _leftRestPosition != null)
        {
            _bossHandLeft.transform.position = _leftRestPosition.position;
        }

        if (_bossHandRight != null && _rightRestPosition != null)
        {
            _bossHandRight.transform.position = _rightRestPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking)
        {
            return;
        }

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

    void Attack()
    {
        int leftIndex = PickIndex(LeftAllowedIndices, _previousLeftIndex);
        int rightIndex = PickIndex(RightAllowedIndices, _previousRightIndex, leftIndex);

        _previousLeftIndex = leftIndex;
        _previousRightIndex = rightIndex;

        StartCoroutine(AttackBothRoutine(leftIndex, rightIndex));
    }

    // Picks a random index from the allowed set, avoiding the previous index and
    // (if possible) the excludeIndex used by the other hand so both hands don't attack
    // the same position at the same time.
    int PickIndex(int[] allowed, int previousIndex, int excludeIndex = -1)
    {
        List<int> candidates = new List<int>();
        foreach (int idx in allowed)
        {
            if (idx != excludeIndex)
            {
                candidates.Add(idx);
            }
        }

        // if excluding the other hand's index leaves nothing, allow it anyway (only shared slot left)
        if (candidates.Count == 0)
        {
            candidates.AddRange(allowed);
        }

        List<int> preferred = candidates.FindAll(idx => idx != previousIndex);
        if (preferred.Count > 0)
        {
            candidates = preferred;
        }

        // center line (index 1) only has a 25% chance of being picked, if there's another option
        if (candidates.Contains(CenterIndex) && rand.NextDouble() >= CenterLineChance)
        {
            List<int> nonCenter = candidates.FindAll(idx => idx != CenterIndex);
            if (nonCenter.Count > 0)
            {
                candidates = nonCenter;
            }
        }

        return candidates[rand.Next(0, candidates.Count)];
    }

    IEnumerator AttackBothRoutine(int leftIndex, int rightIndex)
    {
        _isAttacking = true;

        Coroutine leftRoutine = null;
        Coroutine rightRoutine = null;

        if (_bossHandLeft != null)
        {
            leftRoutine = StartCoroutine(AttackRoutine(_bossHandLeft, _leftRestPosition, _handPrepPosition[leftIndex], _jumpPositions[leftIndex]));
        }

        if (_bossHandRight != null)
        {
            rightRoutine = StartCoroutine(AttackRoutine(_bossHandRight, _rightRestPosition, _handPrepPosition[rightIndex], _jumpPositions[rightIndex]));
        }

        if (leftRoutine != null) yield return leftRoutine;
        if (rightRoutine != null) yield return rightRoutine;

        _isAttacking = false;
    }

    IEnumerator AttackRoutine(GameObject hand, Transform restPosition, Transform prepPos, Transform targetPos)
    {
        Debug.Log("Moving boss hand to prep position");
        yield return StartCoroutine(MoveHand(hand, prepPos.position));

        Vector3 windUpDirection = (targetPos.position - prepPos.position).normalized;
        Vector3 windUpPosition = prepPos.position - windUpDirection * _windUpPullbackDistance;

        Debug.Log("Winding up before attack");
        yield return StartCoroutine(MoveHand(hand, windUpPosition));
        yield return new WaitForSeconds(_windUpPauseDuration);

        Debug.Log("Moving boss hand to jump position");
        yield return StartCoroutine(MoveHand(hand, targetPos.position));

        Debug.Log("Returning boss hand to rest position");
        yield return StartCoroutine(MoveHand(hand, restPosition.position));
    }

    IEnumerator MoveHand(GameObject hand, Vector3 targetPosition)
    {
       
        OnHandMovementStarted?.Invoke(hand, targetPosition); /// CF added this to give other scripts something to check against 

        while (Vector3.Distance(hand.transform.position, targetPosition) > 0.001f)
        {
            hand.transform.position = Vector3.MoveTowards(
                hand.transform.position,
                targetPosition,
                _handMoveSpeed * Time.deltaTime);

            yield return null;
        }

        hand.transform.position = targetPosition;
    }
}

