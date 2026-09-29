using UnityEngine;
using UnityEngine.UI;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Roman Zhurakhov - wires up the Menu scene's buttons to LevelManager.Instance.
/// LevelManager now persists via DontDestroyOnLoad, so Instance is guaranteed
/// to already exist by the time this scene's Start() runs - no waiting needed.
/// </summary>
#endregion
public class MenuButtons : MonoBehaviour
{
    [Header("Assign these from THIS scene (Menu) - safe, they live here")]
    [SerializeField] private Button startLevel01Button;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button testLevel01Button;
    [SerializeField] private Button testLevel02Button;
    [SerializeField] private Button testLevel03Button;
    [SerializeField] private Button testBossButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        LevelManager levelManager = LevelManager.Instance;

        if (levelManager == null)
        {
            Debug.LogError("MenuButtons: LevelManager.Instance is null. " +
                            "Make sure the 'hud' scene is loaded first (Build Index 0).");
            return;
        }

        if (startLevel01Button != null)
            startLevel01Button.onClick.AddListener(levelManager.onLevel01);

        if (tutorialButton != null)
            tutorialButton.onClick.AddListener(levelManager.onTutorial);

        if (testLevel01Button != null)
            testLevel01Button.onClick.AddListener(levelManager.onLevel01);

        if (testLevel02Button != null)
            testLevel02Button.onClick.AddListener(levelManager.onLevel02);

        if (testLevel03Button != null)
            testLevel03Button.onClick.AddListener(levelManager.onLevel03);

        if (testBossButton != null)
            testBossButton.onClick.AddListener(levelManager.onBoss);

        if (quitButton != null)
            quitButton.onClick.AddListener(Application.Quit);
    }
}