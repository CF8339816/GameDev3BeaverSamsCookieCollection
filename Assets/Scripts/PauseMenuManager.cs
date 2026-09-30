using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Roman Zhurakhov - repurposed this script as the Pause Menu:
/// Resume, Volume slider and Main Menu. Win/lose logic moved to GameResultManager.
/// </summary>
#endregion
public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameResultManager gameResultManager;

    [Tooltip("Scenes where pausing is not allowed")]
    [SerializeField] private string[] noPauseScenes = { "Menu", "Tutorial" };

    private const string VolumePrefKey = "MasterVolume";
    private bool isPaused = false;

    private void Awake()
    {
        if (levelManager == null)
            levelManager = FindFirstObjectByType<LevelManager>();

        if (gameResultManager == null)
            gameResultManager = FindFirstObjectByType<GameResultManager>();

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanPause())
        {
            TogglePause();
        }
    }

    private bool CanPause()
    {
        if (gameResultManager != null && gameResultManager.IsResultShown)
            return false; // don't allow pausing while the win/lose screen is up

        string activeSceneName = SceneManager.GetActiveScene().name;
        foreach (string sceneName in noPauseScenes)
        {
            if (activeSceneName == sceneName)
                return false;
        }

        return true;
    }

    public void TogglePause()
    {
        if (isPaused)
        { 
            Resume();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false; // Hide the cursor when resuming the game
        }
        else
        {
            Pause();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true; // Show the cursor when pausing the game
        }
    }

    public void Pause()
    {
        if (!CanPause()) return; // extra safety net if called from elsewhere (e.g. a UI Pause button)

        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }

    public void GoToMainMenu()
    {
        Resume();

        if (levelManager != null)
        {
            levelManager.onMenu();
        }
        else
        {
            Debug.LogError("PauseMenuManager is missing its LevelManager reference! Assign it in the Inspector.");
        }
    }
}