using UnityEngine;
using UnityEngine.UI;

public class TutorialButtons : MonoBehaviour
{
    [SerializeField] private Button tutorialStart;
    [SerializeField] private Button tutorialBackToMenu;

    private void Start()
    {
        LevelManager levelManager = LevelManager.Instance;

        if (levelManager == null)
        {
            Debug.LogError("MenuButtons: LevelManager.Instance is null. " +
                            "Make sure the 'hud' scene is loaded first (Build Index 0).");
            return;
        }

        tutorialStart.onClick.AddListener(levelManager.onLevel01);
        tutorialBackToMenu.onClick.AddListener(levelManager.onMenu);
    }
}
