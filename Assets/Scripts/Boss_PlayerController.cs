using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss_PlayerController : MonoBehaviour
{
    [SerializeField] List<Transform> _jumpPositions = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            transform.position = _jumpPositions[0].position;
        if (Input.GetKeyDown(KeyCode.Space))
            transform.position = _jumpPositions[1].position;
        if (Input.GetMouseButtonDown(1))
            transform.position = _jumpPositions[2].position;
    }
}
