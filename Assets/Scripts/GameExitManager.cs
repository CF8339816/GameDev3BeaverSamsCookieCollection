using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// Roman Zhurakhov - fixed missing EventManager reference and removed
/// dependency on the deleted EventManager.MaxCalories field; win threshold
/// is now configured directly on this component.
/// </summary>
#endregion
public class GameExitManager : MonoBehaviour
{
    public float ExitDelay = 5f;
    public TextMeshProUGUI exitCountdownText;

    [Tooltip("How many total cookies the player needs to have collected to win the game")]
    [SerializeField] private int cookiesNeededToWin = 40;

    private float Countdown;
    private bool isExiting = false;
    [SerializeField] private EventManager eventManager;
    [SerializeField] public Button menu;
    [SerializeField] public Button restart;
    [SerializeField] public Button quit;

    void Awake()
    {
        Countdown = ExitDelay;

        if (eventManager == null)
            eventManager = FindFirstObjectByType<EventManager>();

        if (exitCountdownText != null)
        {
            exitCountdownText.text = "";
        }
    }

    public void Wincheck()
    {
        if (eventManager == null)
        {
            Debug.LogError("GameExitManager is missing its EventManager reference! Assign it in the Inspector.");
            return;
        }

        if (eventManager.ItemsCount >= cookiesNeededToWin)
        {
            eventManager.DisplayInfoMessage("You have managed to hold on to enough cookies for the winter you survive your hibernation!");
        }
        else
        {
            eventManager.DisplayInfoMessage("Try as you might you could not defeat the Dangle Dragon,\n too many of your cookies were 'grabbie grabbed' \n you just don't have the calories available to survive the winter, you almost make it but freeze to death in early spring...  \n you are mourned by the other beavers who thought you were a little wierd anyway");
        }

        ButtonVisisbility();
    }

    public void ButtonVisisbility()
    {
        menu.interactable = true;
        quit.interactable = true;
        restart.interactable = true;
    }

    public void Ongameexit()
    {
        if (!isExiting)
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
