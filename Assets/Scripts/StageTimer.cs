using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class StageTimer : MonoBehaviour
{
    [SerializeField] public TMP_Text stageTimer;
    [SerializeField] public TMP_Text infoBox;
    [SerializeField] public LevelManager levelManager;
    private float timeRemaining;
    private bool isTimerRunning = false;

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer();
            }
            else
            {
                // Timer reached zero
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimer();
                Timer0();
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

    public void ResetAndStartTimer(float newTimeAllocation)
    {
        timeRemaining = newTimeAllocation;
        isTimerRunning = true;
    }
    private void Timer0()
    {
        infoBox.text = " Time's Up the Cookies have gone bad. Time to check the next set of rooms. ";
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


}