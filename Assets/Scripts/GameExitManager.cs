using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// 
/// </summary>
#endregion
public class GameExitManager : MonoBehaviour
{
    public float ExitDelay = 5f;
    public TextMeshProUGUI exitCountdownText;
    private float Countdown;
    private bool isExiting = false;
    //private EventManager eventManager;
    //private AlphaPOC_BossCombatController alphaPOC_BossCombatController;
    //private AddAudio addAudio;

    void Awake()
    {
        Countdown = ExitDelay;
        if (exitCountdownText != null)
        {
            exitCountdownText.text = "";
        }
        // addAudio = Object.FindFirstObjectByType<AddAudio>(); // initalizes
        //eventManager = Object.FindFirstObjectByType<EventManager>(); // initalizes
        //alphaPOC_BossCombatController = Object.FindFirstObjectByType<AlphaPOC_BossCombatController>();// find and initalize

    }
    //public void Wincheck()
    //{
    //    if (eventManager.ItemsCount >= eventManager.MaxCalories)
    //    {
    //        alphaPOC_BossCombatController.DisplayBossFightMessage("You have managed to hold on to enough cookies for the winter you survive your hibernation!");
    //    }
    //    else
    //    {
    //        alphaPOC_BossCombatController.DisplayBossFightMessage("Try as you might you could not defeat the Dangle Dragon,\n too many of your cookies were 'grabbie grabbed' \n you just don't have the calories available to survive the winter, you almost make it but freeze to death in early spring...  \n you are mourned by the other beavers who thought you were a little wierd anyway");
    //        addAudio.OnLoss();
    //    }

    //}

      public void Ongameexit()
    {
        if ( !isExiting)
        {

            isExiting = true;
            StartCoroutine(StartExitCountdown());

        }


    }
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
