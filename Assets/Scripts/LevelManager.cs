using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review Roman Zhurakhov - converted level switching to scene loading,
/// and cookie targets are now defined per level (9 / 15 / 21).
/// </summary>
#endregion

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [System.Serializable]
    public struct LevelDefinition
    {
        public string SceneName;
        public int CookieTarget;
    }

    [Header("Level order and cookie goals per level")]
    [SerializeField] private LevelDefinition[] _levels = new LevelDefinition[]
    {
        new LevelDefinition { SceneName = "Level01", CookieTarget = 9 },
        new LevelDefinition { SceneName = "Level02", CookieTarget = 15 },
        new LevelDefinition { SceneName = "Level03", CookieTarget = 21 },
    };

    [SerializeField] private string _bossSceneName = "BossStage";
    [SerializeField] private string _tutorialSceneName = "Tutorial";
    [SerializeField] private string _menuSceneName = "Menu";

    [SerializeField] private EventManager eventManager; //added to ensure level manager can find the event manager to tell it when to initalize stages

    public GameObject HUD;

    private int _currentLevelIndex = -1;
    private string _currentSceneName;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (eventManager == null)
            eventManager = FindFirstObjectByType<EventManager>();
    }

    public void Start()
    {
        HUD.SetActive(false);
        LoadMenu();
    }

    private IEnumerator LoadSceneRoutine(string sceneName, bool showHUD, int cookieTarget)
    {
        if (!string.IsNullOrEmpty(_currentSceneName))
        {
            yield return SceneManager.UnloadSceneAsync(_currentSceneName);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _currentSceneName = sceneName;

        HUD.SetActive(showHUD);

        // Generate cookies for this level, capped at cookieTarget.
        LevelGeneration levelGeneration = FindFirstObjectByType<LevelGeneration>();
        if (levelGeneration != null)
        {
            levelGeneration.GenerateLevel(cookieTarget);
        }

        if (eventManager != null)
        {
            eventManager.OnLevelChange(cookieTarget);
        }
    }

    public void LoadNextChronologicalLevel()  // loads stages in next chronological order
    {
        int nextIndex = _currentLevelIndex + 1;

        if (nextIndex < _levels.Length)
        {
            LoadLevelByIndex(nextIndex);
        }
        else
        {
            // Ran out of numbered levels - go to the boss stage.
            LoadBoss();
        }
    }

    public void LoadLevelByIndex(int index)
    {
        if (index < 0 || index >= _levels.Length)
        {
            Debug.LogError($"LevelManager: level index {index} is out of range.");
            return;
        }

        _currentLevelIndex = index;
        LevelDefinition level = _levels[index];
        StartCoroutine(LoadSceneRoutine(level.SceneName, showHUD: true, level.CookieTarget));
    }

    public void LoadMenu()
    {
        _currentLevelIndex = -1;
        StartCoroutine(LoadSceneRoutine(_menuSceneName, showHUD: false, cookieTarget: 0));
    }

    public void LoadTutorial()
    {
        _currentLevelIndex = -1;
        StartCoroutine(LoadSceneRoutine(_tutorialSceneName, showHUD: false, cookieTarget: 0));
    }

    public void LoadBoss()
    {
        _currentLevelIndex = _levels.Length; // past the last numbered level
        StartCoroutine(LoadSceneRoutine(_bossSceneName, showHUD: true, cookieTarget: 0));
    }

    // Convenience wrappers matching old UI Button hookups (Level01/02/03 buttons, etc.)
    public void onLevel01() => LoadLevelByIndex(0);
    public void onLevel02() => LoadLevelByIndex(1);
    public void onLevel03() => LoadLevelByIndex(2);
    public void onTutorial() => LoadTutorial();
    public void onMenu() => LoadMenu();
    public void onBoss() => LoadBoss();
}
