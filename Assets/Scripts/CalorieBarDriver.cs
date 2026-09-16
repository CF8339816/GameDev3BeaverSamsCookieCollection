using System;
using TMPro;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static LevelManager;
using UnityEngine.UI;
using UnityEngine.Android;

public class CalorieBarDriver : MonoBehaviour
{

    [SerializeField] private TMP_Text cookieCount;
    [SerializeField] private TMP_Text calerieCount;
    [SerializeField] private Slider calorieSlider;
    [SerializeField] private int MaxCalories = 40;

    [SerializeField] private LevelManager levelManager;

    public TextMeshProUGUI textCookieCount;

    

    private int currentCalories = 0;
    private int CookieCount;
    private int CalorieCount;
    public GameObject currentActiveLevel;
    public int CookiesPerLevel { get; set; }

    void Start()
    {
        calorieSlider.value = 0;
        calorieSlider.minValue = 0;
        calorieSlider.maxValue = 40;

    }

    public void cookieCalories()
    {
       
        if (currentCalories >= MaxCalories) return; //  hard stop at 40 even though there are more cookies

        currentCalories++;

       
        calorieSlider.value = currentCalories;// sets calery slider value to  currentcalories variable

        Debug.Log($"Collected: {currentCalories}/{MaxCalories}"); //verifies  the calorie slider addition when cookies are picked up

        if (currentCalories >= MaxCalories)
        {
           whenMaxCalories();
        }
    }

    private  void whenMaxCalories()
    {
        //TriggerBoss level fight
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) //checks obj ffor PickUp tag
        {
            other.gameObject.SetActive(false); //deactivates obj when collided
            cookieCalories(); //adds 1 to calorie slider value and  updates the slider  based on the method
            CookieCount++;//adds 1 to count when picked up
            SetCookieValue();   
        }
    }
   

      void setCookiesPerLevel()
      {
        GameObject activeLevel = levelManager.currentActiveLevel;// sets  variable for active level directly from the active level state in level manager

        if (activeLevel == levelManager.Level01)
        {
            CookiesPerLevel = 10;
        }
        if (activeLevel == levelManager.Level02)
        { 
            CookiesPerLevel = 15;
        }
        if (activeLevel == levelManager.Level03)
        {
            CookiesPerLevel = 20;
        }
        if (activeLevel == levelManager.BossFight)
        {
            CookiesPerLevel = 0;
        }
        if (activeLevel == levelManager.Tutorial)
        {
            CookiesPerLevel = 5;
        }

        else
        {
            CookiesPerLevel = 0;
        }

      }
     void SetCookieValue()
     {
        textCookieCount.text = "Cookie Count: " + CookieCount.ToString() ; // sets count to output to string

        if (CookieCount == CookiesPerLevel)
        {
            // Run completion message and start next level
        }

       
     }

}



