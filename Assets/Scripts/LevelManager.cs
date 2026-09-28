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
/// code review Roman Zhurakhov - scene-based level switching; EventManager
/// re-finds its scene-scoped references automatically after every load.
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

    [SerializeField] private EventManager eventManager;

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
        LoadMenu();
    }

    private IEnumerator LoadSceneRoutine(string sceneName, int cookieTarget)
    {
        if (!string.IsNullOrEmpty(_currentSceneName))
        {
            yield return SceneManager.UnloadSceneAsync(_currentSceneName);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _currentSceneName = sceneName;

        // Only numbered levels have procedural generation.
        LevelGeneration levelGeneration = FindFirstObjectByType<LevelGeneration>();
        if (levelGeneration != null)
        {
            levelGeneration.GenerateLevel(cookieTarget);
        }

        if (eventManager != null)
        {
            // This re-finds BossController/Boss_PlayerController/AddAudio/IconVisibility
            // for whatever scene was just loaded, and resets the cookie goal + timer.
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
        StartCoroutine(LoadSceneRoutine(level.SceneName, level.CookieTarget));
    }

    public void LoadMenu()
    {
        _currentLevelIndex = -1;
        StartCoroutine(LoadSceneRoutine(_menuSceneName, cookieTarget: 0));
    }

    public void LoadTutorial()
    {
        _currentLevelIndex = -1;
        StartCoroutine(LoadSceneRoutine(_tutorialSceneName, cookieTarget: 0));
    }

    public void LoadBoss()
    {
        _currentLevelIndex = _levels.Length;
        StartCoroutine(LoadSceneRoutine(_bossSceneName, cookieTarget: 0));
    }

    public void onLevel01() => LoadLevelByIndex(0);
    public void onLevel02() => LoadLevelByIndex(1);
    public void onLevel03() => LoadLevelByIndex(2);
    public void onTutorial() => LoadTutorial();
    public void onMenu() => LoadMenu();
    public void onBoss() => LoadBoss();
}
