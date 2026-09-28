using UnityEngine;
using UnityEngine.UI;

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

    private const string VolumePrefKey = "MasterVolume";
    private bool isPaused = false;

    private void Awake()
    {
        if (levelManager == null)
            levelManager = FindFirstObjectByType<LevelManager>();

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
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
        Resume(); // make sure timeScale is restored before switching scenes

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