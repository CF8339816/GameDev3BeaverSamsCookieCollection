using TMPro;
using UnityEngine;
using System.Collections;

public class GameExitManager : MonoBehaviour
{
    public float ExitDelay = 5f;
    public TextMeshProUGUI exitCountdownText;
    private float Countdown;
    private bool isExiting = false;


    void Awake()
    {
        Countdown = ExitDelay;

        if (exitCountdownText != null)
        {
            exitCountdownText.text = "";
        }
    }

   

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && !isExiting)
    //    {

    //        isExiting = true;
    //        StartCoroutine(StartExitCountdown());

    //    }


    //}
    public IEnumerator StartExitCountdown()
    {
        while (Countdown > 0)
        {
            exitCountdownText.text = "Game Exit in: " + Mathf.Ceil(Countdown).ToString();// displays the countdown output in an always rounded up to whole int
            yield return null;

            Countdown -= Time.deltaTime;
        }

        exitCountdownText.text = "Exiting...";
        ExitGame();
    }



    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

        // Works in a built application
        Application.Quit();

        // Works inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
