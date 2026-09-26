using UnityEngine;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review Roman Zhurakhov - updated debug hotkeys to match the new
/// scene-based LevelManager API (no more Level01/02/03 GameObjects).
/// </summary>
#endregion

public class Menu : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    void Awake()
    {
        if (levelManager == null)
            levelManager = FindFirstObjectByType<LevelManager>();
    }

    void Update()
    {
        HandleDebugInputs();
    }

    private void HandleDebugInputs()
    {
        if (levelManager == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) levelManager.onLevel01();
        if (Input.GetKeyDown(KeyCode.Alpha2)) levelManager.onLevel02();
        if (Input.GetKeyDown(KeyCode.Alpha3)) levelManager.onLevel03();
    }
}
