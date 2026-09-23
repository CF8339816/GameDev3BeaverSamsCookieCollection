using UnityEngine;


#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// 
/// </summary>
#endregion


public class Menu : MonoBehaviour
{

 private LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleDebugInputs()
    {
        if (levelManager == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) { levelManager.levelChange(levelManager.Level01); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { levelManager.levelChange(levelManager.Level02); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { levelManager.levelChange(levelManager.Level03); }
    }
}
