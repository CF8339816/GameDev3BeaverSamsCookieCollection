using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    private System.Random rand = new System.Random();
    private CalorieBarDriver calorieBarDriver;
    Outline outline;
    public string message;

    public UnityEvent onInteraction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void EatCookie()
    {
        calorieBarDriver.OnEat();
        gameObject.SetActive(false);
    }
}
