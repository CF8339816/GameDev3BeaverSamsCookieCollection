using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultButtons : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelManager levelManager = LevelManager.Instance;

        if (levelManager == null)
        {
            Debug.LogError("MenuButtons: LevelManager.Instance is null. " +
                            "Make sure the 'hud' scene is loaded first (Build Index 0).");
            return;
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(levelManager.onLevel01);

        if (menuButton != null)
            menuButton.onClick.AddListener(levelManager.onMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
