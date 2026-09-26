using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CF- added a proxy is attacking bool value that will alow the event manager to check the bool status externally wiuthout the ability to change it 
/// </summary>

public class Boss_PlayerController : MonoBehaviour
{
    [SerializeField] List<Transform> _jumpPositions = new List<Transform>();

    [Header("Jump Settings")]
    [SerializeField] float _jumpDuration = 0.3f;
    [SerializeField] float _jumpHeight = 1f;

    Coroutine _jumpRoutine;

    public static event Action<Vector3> OnPlayerJumpStarted; /// CF added this to give other scripts something to check against 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = _jumpPositions[1].position; // Start at the center position
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            JumpTo(_jumpPositions[0].position);
        if (Input.GetKeyDown(KeyCode.Space))
            JumpTo(_jumpPositions[1].position);
        if (Input.GetMouseButtonDown(1))
            JumpTo(_jumpPositions[2].position);
    }

    void JumpTo(Vector3 targetPosition)
    {
        if (_jumpRoutine != null)
        {
            StopCoroutine(_jumpRoutine);
        }

        _jumpRoutine = StartCoroutine(JumpRoutine(targetPosition));
    }

    IEnumerator JumpRoutine(Vector3 targetPosition)
    {
        OnPlayerJumpStarted?.Invoke(targetPosition);  /// CF added this to give other scripts something to check against 

        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < _jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _jumpDuration);

            Vector3 flatPosition = Vector3.Lerp(startPosition, targetPosition, t);
            float arc = Mathf.Sin(t * Mathf.PI) * _jumpHeight;

            transform.position = flatPosition + Vector3.up * arc;

            yield return null;
        }

        transform.position = targetPosition;
        _jumpRoutine = null;
    }
}

