using TMPro;
using UnityEngine;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Roman Zhurakhov - extracted win/lose check logic out of GameExitManager
/// (which is now a Pause Menu) so each script has a single responsibility.
/// </summary>
#endregion
public class GameResultManager : MonoBehaviour
{
    [Tooltip("How many total cookies the player needs to have collected to win the game")]
    [SerializeField] private int cookiesNeededToWin = 25;

    public int CookiesNeededToWin => cookiesNeededToWin;

    [SerializeField] private EventManager eventManager;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;

    private void Awake()
    {
        if (eventManager == null)
            eventManager = FindFirstObjectByType<EventManager>();

        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    public bool IsResultShown { get; private set; } = false;

    /// <summary>
    /// Called by EventManager once the boss encounter ends (boss stops attacking).
    /// </summary>
    public void CheckWin()
    {
        if (eventManager == null)
        {
            Debug.LogError("GameResultManager is missing its EventManager reference! Assign it in the Inspector.");
            return;
        }

        bool won = eventManager.ItemsCount >= cookiesNeededToWin;

        string message = won
            ? "You have managed to hold on to enough cookies for the winter you survive your hibernation!"
            : "Try as you might you could not defeat the Dangle Dragon,\n too many of your cookies were 'grabbie grabbed' \n you just don't have the calories available to survive the winter, you almost make it but freeze to death in early spring...  \n you are mourned by the other beavers who thought you were a little wierd anyway";

        eventManager.DisplayInfoMessage(message);

        IsResultShown = true;

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            if (resultText != null)
                resultText.text = message;
        }
    }

    public void ResetResultState()
    {
        IsResultShown = false;

        if (resultPanel != null)
            resultPanel.SetActive(false);
    }
}
