using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private Slider calorieSlider;

    [SerializeField] public float LevelTimer = 30f;
    public float Countdown;

    [SerializeField] public LevelManager levelManager;
    [SerializeField] private GameResultManager gameResultManager;

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
        BossController.OnAttackResolved += HandleBossAttackResolved; // reacts to hit/dodge at the end of each attack
    }

    private void OnDisable()
    {
        PlayerInteraction.OnCookieEaten -= updateHUDFromCookieAction;
        BossController.OnHandMovementStarted -= HandleHandMovement;
        Boss_PlayerController.OnPlayerJumpStarted -= HandlePlayerJump;
        BossController.OnAttackResolved -= HandleBossAttackResolved;
    }

    private void HandleBossAttackResolved(bool playerWasHit)
    {
        Debug.Log($"EventManager.HandleBossAttackResolved: playerWasHit={playerWasHit}");

        if (playerWasHit)
        {
            ItemsCount = Mathf.Max(0, ItemsCount - 1); // never go below 0, max 1 cookie lost per attack
            UpdateCalorieSlider();
            DisplayInfoMessage("oh Noes the Dangle Dragon has snached a cookie!", 1f);
        }
        else
        {
            DisplayInfoMessage("You dodged the Dangle Dragon's grabbie grab it got no cookies this time", 1f);
        }
    }

    private void ResetBossFlags()
    {
        isBossStage = false;
        bossResultShown = false;
        wasBossAttacking = false; // reset attack-edge tracking for the new fight
    }

    void Update()
    {
        checkTimer();
        SetItemsValue();
        // CheckBossGrab() removed - hit/dodge now resolved via BossController.OnAttackResolved event
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

        UpdateCalorieSlider(); // keep slider max in sync in case cookiesNeededToWin changed
    }

    public void DisplayInfoMessage(string message, float duration = 4f)// formats info box messages to use display clear timer instead of being on screen dynamically
    {
        if (textInfoBox == null) return;

        textInfoBox.text = message; //defines new message variable

        if (activeTextTimer != null) // stops any currently running timer upon new one started
        {
            StopCoroutine(activeTextTimer);
        }
        activeTextTimer = StartCoroutine(ClearTextBoxAfterDelay(duration));
    }

    private void UpdateCalorieSlider()
    {
        if (calorieSlider == null) return;

        int maxCalories = gameResultManager != null ? gameResultManager.CookiesNeededToWin : 40;

        calorieSlider.minValue = 0;
        calorieSlider.maxValue = maxCalories;
        calorieSlider.value = ItemsCount;
    }

    public void updateHUDFromCookieAction()
    {
        ItemsCount++;//adds 1 to max game count when picked up
        ItemPerLevelCount++;//adds 1 to level count when picked up

        if (addAudio != null)
            addAudio.OnNom();

        if (iconVisibility != null)
            iconVisibility.FlashVisible();

        UpdateCalorieSlider();

        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (MaxItemPerLevel <= 0)
            return; // no cookie goal set (Menu/Tutorial/Boss) - nothing to check

        if (ItemPerLevelCount >= MaxItemPerLevel)
        {
            DisplayInfoMessage("You've collected all the cookies here! Let's check the next set of rooms.");

            StopTimerCoroutine(); // stop the countdown, we're moving on early

            if (levelManager != null)
            {
                levelManager.LoadNextChronologicalLevel();
            }
            else
            {
                Debug.LogError("EventManager is missing its LevelManager reference! Assign it in the Inspector.");
            }
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

    private bool isBossStage = false;
    private bool bossResultShown = false;

    public void checkTimer()
    {
        if (isBossStage)
        {
            if (!bossResultShown && Countdown < 1)
            {
                HandleBossFightEnd();
            }
            return;
        }

        if (MaxItemPerLevel <= 0)
            return; // no cookie goal means we're in Menu/Tutorial - no countdown here

        if (Countdown < 1)
        {
            Timer0();
        }
    }

    private void HandleBossFightEnd()
    {
        bossResultShown = true; // guard so this only ever fires once per boss fight
        StopTimerCoroutine();

        if (bossController != null)
            bossController.EndFight(); // stop attacking, snap hands back to rest

        if (gameResultManager != null)
            gameResultManager.CheckWin();
        else
            Debug.LogError("EventManager is missing its GameResultManager reference!");
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

    /// <summary>
    /// Called by LevelManager specifically when loading the boss stage.
    /// Starts a fight-duration countdown; the result panel only appears once it hits zero.
    /// </summary>
    public void StartBossStage(float fightDuration)
    {
        isBossStage = true;
        bossResultShown = false;
        wasBossAttacking = false; // reset attack-edge tracking for the new fight

        RefreshSceneReferences();

        DisplayInfoMessage("The Dangle Dragon appears! Dodge its grabs!");

        Countdown = fightDuration;
        StopTimerCoroutine();
        countdownCoroutine = StartCoroutine(ResetAndStartTimer());
    }

    public void OnLevelChange(int cookieTargetForLevel)//called by LevelManager after a scene is loaded
    {
        isBossStage = false; // leaving boss mode whenever a normal level/menu loads
        bossResultShown = false;

        MaxItemPerLevel = cookieTargetForLevel;
        ItemPerLevelCount = 0;

        RefreshSceneReferences();

        SetItemsValue();

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

    private bool wasBossAttacking = false;

    public void CheckBossGrab()
    {
        if (bossController == null || boss_playerController == null)
            return;

        bool isAttackingNow = bossController.IsAttacking;

        // Only resolve hit/dodge exactly once, right when the attack begins.
        if (isAttackingNow && !wasBossAttacking)
        {
            if ((LeftHandTarget == PlayerJumpTarget) || (RightHandTarget == PlayerJumpTarget))
            {
                ItemsCount--;
                DisplayInfoMessage("oh Noes the Dangle Dragon has snached a cookie!", 1f);
            }
            else
            {
                DisplayInfoMessage("You dodged the Dangle Dragon's grabbie grab it got no cookies this time", 1f);
            }
        }

        wasBossAttacking = isAttackingNow;
    }

    public void SetItemsValue()  // sets the text output for the stage
    {
        if (textItemsCount == null) return;

        if (isBossStage)
        {
            int cookiesNeededToWin = gameResultManager != null ? gameResultManager.CookiesNeededToWin : 40;
            textItemsCount.text = "Cookies: " + ItemsCount.ToString() + "/" + cookiesNeededToWin.ToString();
        }
        else
        {
            textItemsCount.text = "Item Count: " + ItemPerLevelCount.ToString() + "/" + MaxItemPerLevel.ToString();
        }
    }
}