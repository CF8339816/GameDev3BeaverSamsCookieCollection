using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventManager : MonoBehaviour
{

    [SerializeField] private TMP_Text CookieCountPerLevel;
    [SerializeField] private TMP_Text InfoBox;
    [SerializeField] private Slider CalorieCounter;
    [SerializeField] private int MaxCalories = 40;
    [SerializeField] private int MaxCookies = 45;
    [SerializeField] public LevelManager levelManager;
    [SerializeField] private Slider bloatSlider;
    [SerializeField] private int MaxBloat = 5;

    public TextMeshProUGUI textItemsCount;
    public TextMeshProUGUI textInfoBox;
    [SerializeField] public TMP_Text stageTimer;
    public TextMeshProUGUI textStageTimer;


    public float newTimeAllocation;
    private float timeRemaining;
    private bool isTimerRunning = false;

    private int currentItems = 0;

    private int ItemPerLevelCount = 0;
    private int ItemsCount;
    private GameObject currentActiveLevel;
    public int MaxItemPerLevel;
    private GameObject activeLevel;
    private IEnumerator ClearTextBoxAfterDelay(float delay) //setting up diisplay timer for info box messages
    {
        yield return new WaitForSeconds(delay);  // allows for time delay set in seconds

        textInfoBox.text = ""; //Sets cleared message
    }

    private Coroutine activeTextTimer; //defines timer coroutine

    void Start()
    {
        CalorieCounter.value = 0;
        CalorieCounter.minValue = 0;
        CalorieCounter.maxValue = 40;
        ItemPerLevelCount = 0;
        bloatSlider.value = 0;
        bloatSlider.minValue = 0;
        bloatSlider.maxValue = 5;


        if (levelManager != null)
        {
            OnLevelChange(levelManager.currentActiveLevel);
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E)) //checks for keypress to simulate item pickup
        //{


        //    ItemsCount++;//adds 1 to max game count when picked up
        //    ItemPerLevelCount++;//adds 1 to level count when picked up
        //    SetItemsValue();
        //    collectedItems(); //checks if max items reached for game  checks for max per level items for level change

        //}

        updateHUD();
        UpdateTimer();

        if (Input.GetKeyDown(KeyCode.Alpha1) && levelManager != null) //checks for keypress to simulate level change manually
        {
            levelManager.levelChange(levelManager.Level01);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && levelManager != null) //checks for keypress to simulate level change manually
        {
            levelManager.levelChange(levelManager.Level02);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && levelManager != null) //checks for keypress to simulate level change manually
        {
            levelManager.levelChange(levelManager.Level03);
        }

        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimer();
                Timer0();
            }
        }




    }

    public void DisplayInfoMessage(string message)// formats info box messages to utalize display clear timer instead of being on screen dynamically
    {

        textInfoBox.text = message; //defines new message variavle name

        if (activeTextTimer != null) // stops any currently running timer  upon new one started
        {
            StopCoroutine(activeTextTimer);
        }

        activeTextTimer = StartCoroutine(ClearTextBoxAfterDelay(4f)); //starts newly defined timer (currently 4 sec)
    }

   
    public void updateHUD()
    {
        if (Input.GetKeyDown(KeyCode.E)) //checks for keypress to simulate item pickup
        {

            if (gameObject.tag == "Interactable")
            {
                ItemsCount++;//adds 1 to max game count when picked up
                ItemPerLevelCount++;//adds 1 to level count when picked up
                collectedItems(); //checks if max items reached for game  checks for max per level items for level change
            }

        }
    }
    private void UpdateTimer()
    {

        if (timeRemaining < 0) timeRemaining = 0; // prevents negative seconds count 

        int seconds = Mathf.FloorToInt(timeRemaining); // Display seconds
        int milliseconds = Mathf.FloorToInt((timeRemaining - seconds) * 100);// display miliseconds

        stageTimer.text = string.Format("{0:00}:{1:00}", seconds, milliseconds); //output value to timer
    }

    public void ResetAndStartTimer()
    {
        timeRemaining = newTimeAllocation;
        isTimerRunning = true;
    }
    private void Timer0()
    {
        DisplayInfoMessage(" Time's Up the Cookies have gone bad. Time to check the next set of rooms. ");
        //load nerxt stage code
        if (levelManager != null)
        {
            levelManager.LoadNextChronologicalLevel();
        }
        else
        {
            Debug.LogError("StageTimer is missing its LevelManager reference! Assign it in the Inspector.");
        }
    }

    public void OnLevelChange(GameObject targetLevel)//manual  level change   and  reset
    {
        activeLevel = targetLevel;
        ResetAndStartTimer();
        setItemsPerLevel(); // sets  max ipl
        SetItemsValue();  // resets level collection counter
    }

    public void collectedItems()
    {
        currentItems = ItemsCount;  // sets slider value to collected value

        CalorieCounter.value = currentItems;// sets calery slider value to  currentcalories variable

        Debug.Log($"Collected: {currentItems}/{MaxCalories}"); //verifies  the item slider addition whenitems are picked up

        if (currentItems >= MaxCalories)
        {
            DisplayInfoMessage("you have collected all the items Needed  in the game  congrats you win");
            return; //  hard stop at 40 even though there are more cookies
        }


        if (currentItems < MaxCalories)// if items are not at game max checks for  if at level max for level change
        {
            whenMaxPerLevelItems();
        }
    }

    public void whenMaxPerLevelItems() //Triggers stage change loads  next level and resets collection and sets max collection
    {
        if (ItemPerLevelCount == MaxItemPerLevel)
        {
            //DisplayInfoMessage("you have collected all the items on that stage! Let's Collect more here!");
            levelManager.LoadNextChronologicalLevel();

            activeLevel = levelManager.currentActiveLevel;
            ItemPerLevelCount = 0;

            setItemsPerLevel(); // sets  max ipl

            SetItemsValue();  // resets level collection counter


        }

    }


    public void setItemsPerLevel()// sets the max collectable items per level to trrigger stage change
    {

        if (activeLevel == levelManager.Level01)
        {
            DisplayInfoMessage(" Let's Collect items here!");
            MaxItemPerLevel = 10;
        }
        else if (activeLevel == levelManager.Level02)
        {
            DisplayInfoMessage("you have collected all the items on that stage! Let's Collect more here!");
            MaxItemPerLevel = 15;
        }
        else if (activeLevel == levelManager.Level03)
        {
            DisplayInfoMessage("you have collected all the items on that stage! Let's Collect more here!");
            MaxItemPerLevel = 20;
        }

    }
    public void SetItemsValue()  // sets the text output for the stage
    {

        textItemsCount.text = "Item Count: " + ItemPerLevelCount.ToString() + "/" + MaxItemPerLevel.ToString(); // sets count to output to string
    }

}



