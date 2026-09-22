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
    [SerializeField] public TextMeshProUGUI levelCountdownText;
    public TextMeshProUGUI textItemsCount;
    public TextMeshProUGUI textInfoBox;
   
    [SerializeField] private int MaxCalories = 40;
    [SerializeField] private int MaxCookies = 45;
    [SerializeField] public float LevelTimer = 30f;
    public float Countdown;
   
    [SerializeField] public LevelManager levelManager;  
    private GameExitManager gameExitManager;
    private PlayerInteraction playerInteraction;
     
    public int MaxItemPerLevel;
    public float newTimeAllocation;
    private int currentItems = 0;
    private int ItemPerLevelCount = 0;
    private int ItemsCount;
    
    private GameObject currentActiveLevel;
    private GameObject activeLevel;
    private IEnumerator ClearTextBoxAfterDelay(float delay) //setting up diisplay timer for info box messages
    {
        yield return new WaitForSeconds(delay);  // allows for time delay set in seconds

        textInfoBox.text = ""; //Sets cleared message
    }

   private IEnumerator ResetAndStartTimer()
    {
        while (Countdown > 0)
        {

            if (levelCountdownText != null)
            {
                levelCountdownText.text = Mathf.Ceil(Countdown).ToString();// displays the countdown output in an always rounded up to whole int
            }
            yield return null;

            Countdown -= Time.deltaTime;
        }  
    }

     private Coroutine activeTextTimer; //defines timer coroutine for message duration
    private Coroutine countdownCoroutine;  //defines timer coroutine for countdown timer

    private void OnEnable()
    {
        PlayerInteraction.OnCookieEaten += updateHUDFromCookieAction; //listens for player's cookie interaction
    }

    private void OnDisable()
    {
        PlayerInteraction.OnCookieEaten -= updateHUDFromCookieAction; //  stops the cookie listen
    }


    void Start()
    {
        CalorieCounter.value = 0;
        CalorieCounter.minValue = 0;
        CalorieCounter.maxValue = 40;
        ItemPerLevelCount = 0;
       
        if (levelManager != null)
        {
            OnLevelChange(levelManager.currentActiveLevel);
                        
        }


    }

    void Update()
    {
        checkTimer();
        SetItemsValue();
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

   
    public void updateHUDFromCookieAction()
    {
        
        ItemsCount++;//adds 1 to max game count when picked up
        ItemPerLevelCount++;//adds 1 to level count when picked up
        collectedItems(); //checks if max items reached for game  checks for max per level items for level change
       
    }
  
   public void checkTimer()
    { 
         if (Countdown == 0) 
        {
            Timer0();
        }       
    }
    public void Timer0()
    {
        DisplayInfoMessage(" Time's Up the Cookies have gone bad. Time to check the next set of rooms. ");
        
        if (levelManager != null)//load next stage code
        {
            levelManager.LoadNextChronologicalLevel();
        }
        else
        {
            Debug.LogError("StageTimer is missing its LevelManager reference! Assign it in the Inspector.");
        }
    }

    private void StopTimerCoroutine()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }
    public void OnLevelChange(GameObject targetLevel)//manual  level change   and  reset
    {
        activeLevel = targetLevel;
        StartCoroutine(ResetAndStartTimer());
        setItemsPerLevel(); // sets  max ipl
        
        SetItemsValue();  // resets level collection counter

        Countdown = LevelTimer;
        StopTimerCoroutine();
        countdownCoroutine = StartCoroutine(ResetAndStartTimer());
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
           // ItemPerLevelCount = 0;

            setItemsPerLevel(); // sets  max ipl

            SetItemsValue();  // resets level collection counter

           // gameExitManager.StartExitCountdown();
        }
    }
    public void setItemsPerLevel()// sets the max collectable items per level to trrigger stage change
    {
        if (activeLevel == levelManager.Level01)
        {
            DisplayInfoMessage(" Let's Collect cookies here!");
            MaxItemPerLevel = 10;
        }
        else if (activeLevel == levelManager.Level02)
        {
            DisplayInfoMessage("You have collected all the cookies on that stage! Let's Collect more here!");
            MaxItemPerLevel = 15;
        }
        else if (activeLevel == levelManager.Level03)
        {
            DisplayInfoMessage("You have collected all the cookies on that stage! Let's Collect more here!");
            MaxItemPerLevel = 20;
        }
    }
    public void SetItemsValue()  // sets the text output for the stage
    {
        textItemsCount.text = "Item Count: " + ItemPerLevelCount.ToString() + "/" + MaxItemPerLevel.ToString(); // sets count to output to string
    }
}



