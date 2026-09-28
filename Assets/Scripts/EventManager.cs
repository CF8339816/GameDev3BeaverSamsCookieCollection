using TMPro;
using UnityEngine;
using System.Collections;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review Roman Zhurakhov - scene-dependent references (boss scripts, audio,
/// icon visibility) are now auto-found at runtime via RefreshSceneReferences(),
/// called by LevelManager whenever a scene is loaded. Removed unused playerInteraction field.
/// </summary>
#endregion

public class EventManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI levelCountdownText;
    public TextMeshProUGUI textItemsCount;
    public TextMeshProUGUI textInfoBox;

    [SerializeField] public float LevelTimer = 30f;
    public float Countdown;

    [SerializeField] public LevelManager levelManager;
    [SerializeField] private GameResultManager gameResultManager;

    // These live in whichever scene is currently loaded (hud, level, or boss stage),
    // so they can't be wired up in the Inspector ahead of time - they're found at runtime.
    private AddAudio addAudio;
    private IconVisibility iconVisibility;
    private BossController bossController;
    private Boss_PlayerController boss_playerController;

    public int MaxItemPerLevel;
    private int ItemPerLevelCount = 0;
    public int ItemsCount;

    public Vector3 LeftHandTarget { get; private set; }
    public Vector3 RightHandTarget { get; private set; }
    public Vector3 PlayerJumpTarget { get; private set; }

    private Coroutine activeTextTimer; //defines timer coroutine for message duration
    private Coroutine countdownCoroutine;  //defines timer coroutine for countdown timer

    private IEnumerator ClearTextBoxAfterDelay(float delay) //setting up display timer for info box messages
    {
        yield return new WaitForSeconds(delay);  // allows for time delay set in seconds
        textInfoBox.text = ""; //Sets cleared message
    }

    private void HandleHandMovement(GameObject hand, Vector3 targetPosition)
    {
        Debug.Log($"{hand.name} is moving to {targetPosition}");

        if (hand.name.Contains("Left"))// caches positions
        {
            LeftHandTarget = targetPosition;
        }
        else
        {
            RightHandTarget = targetPosition;
        }
    }

    private void HandlePlayerJump(Vector3 targetPosition)
    {
        PlayerJumpTarget = targetPosition;
        Debug.Log($"EventManager caught Player jumping to: {PlayerJumpTarget}");
    }

    private IEnumerator ResetAndStartTimer()
    {
        while (Countdown > 0)
        {
            if (levelCountdownText != null)
            {
                levelCountdownText.text = Mathf.Ceil(Countdown).ToString();// displays the countdown output always rounded up to whole int
            }
            yield return null;

            Countdown -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        PlayerInteraction.OnCookieEaten += updateHUDFromCookieAction; //listens for player's cookie interaction
        BossController.OnHandMovementStarted += HandleHandMovement;  // same but for boss hands
        Boss_PlayerController.OnPlayerJumpStarted += HandlePlayerJump;// same but for player move in boss fight
    }

    private void OnDisable()
    {
        PlayerInteraction.OnCookieEaten -= updateHUDFromCookieAction; //  stops the cookie listen
        BossController.OnHandMovementStarted -= HandleHandMovement;   // same but for boss hands
        Boss_PlayerController.OnPlayerJumpStarted -= HandlePlayerJump;// same but for player move in boss fight
    }

    void Update()
    {
        checkTimer();
        SetItemsValue();

        CheckBossGrab();
    }

    /// <summary>
    /// Called by LevelManager right after any scene finishes loading.
    /// Re-finds scene-scoped components so EventManager always has fresh, valid references
    /// no matter which scene is currently active (hud persists, everything else swaps in/out).
    /// </summary>
    public void RefreshSceneReferences()
    {
        if (addAudio == null)
            addAudio = FindFirstObjectByType<AddAudio>();

        if (iconVisibility == null)
            iconVisibility = FindFirstObjectByType<IconVisibility>();

        if (gameResultManager == null)
            gameResultManager = FindFirstObjectByType<GameResultManager>();

        bossController = FindFirstObjectByType<BossController>();
        boss_playerController = FindFirstObjectByType<Boss_PlayerController>();
    }

    public void DisplayInfoMessage(string message)// formats info box messages to use display clear timer instead of being on screen dynamically
    {
        textInfoBox.text = message; //defines new message variable

        if (activeTextTimer != null) // stops any currently running timer upon new one started
        {
            StopCoroutine(activeTextTimer);
        }
        activeTextTimer = StartCoroutine(ClearTextBoxAfterDelay(4f)); //starts newly defined timer (currently 4 sec)
    }

    public void updateHUDFromCookieAction()
    {
        ItemsCount++;//adds 1 to max game count when picked up
        ItemPerLevelCount++;//adds 1 to level count when picked up

        if (addAudio != null)
            addAudio.OnNom();

        if (iconVisibility != null)
            iconVisibility.FlashVisible();
    }

    public void checkTimer()
    {
        if (Countdown < 1)
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

    public void OnLevelChange(int cookieTargetForLevel)//called by LevelManager after a scene is loaded
    {
        MaxItemPerLevel = cookieTargetForLevel;
        ItemPerLevelCount = 0;

        RefreshSceneReferences(); // re-find scene-scoped components for the newly loaded scene

        SetItemsValue();  // resets level collection counter display

        if (cookieTargetForLevel > 0)
        {
            DisplayInfoMessage(" Let's Collect cookies here!");
            if (iconVisibility != null)
                iconVisibility.idleIcon.SetActive(true);
        }
        else if (iconVisibility != null)
        {
            iconVisibility.idleIcon.SetActive(false);
        }

        Countdown = LevelTimer;
        StopTimerCoroutine();
        countdownCoroutine = StartCoroutine(ResetAndStartTimer());
    }

    public void CheckBossGrab()
    {
        if (bossController == null || boss_playerController == null)
            return;

        if (bossController.IsAttacking == true)
        {
            if ((LeftHandTarget == PlayerJumpTarget) || (RightHandTarget == PlayerJumpTarget))
            {
                ItemsCount--;
                DisplayInfoMessage("oh Noes the Dangle Dragon has snached a cookie!");
            }
        }
        else
        {
            DisplayInfoMessage("You dodged the Dangle Dragon's grabbie grab it got no cookies this time");
        }

        if (bossController.IsAttacking == false)
        {
            DisplayInfoMessage("The Dangle Dragon has gotten frustraited with all your Beaverie jumping around \n it has run away with whatever cookies it could grabbie grab...");
            if (gameResultManager != null)
                gameResultManager.CheckWin();
        }
    }

    public void SetItemsValue()  // sets the text output for the stage
    {
        textItemsCount.text = "Item Count: " + ItemPerLevelCount.ToString() + "/" + MaxItemPerLevel.ToString(); // sets count to output to string
    }
}