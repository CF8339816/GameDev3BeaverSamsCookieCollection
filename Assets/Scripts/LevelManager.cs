using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review Roman Zhurakhov
/// </summary>
#endregion

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject Level01;
    public GameObject Level02;
    public GameObject Level03;
    public GameObject Boss;
    public GameObject Tutorial;
    public GameObject Menu;
    public GameObject currentActiveLevel;
    public GameObject levelToLoad;
    private EventManager eventManager;  //added to ensure level manager can find the event manager to tell it when to initalize stages
    private AddAudio addAudio;
    private AlphaPOC_BossCombatController alphaPOC_BossCombatController;
    public GameObject HUD;
    //public GameObject BSCursor;

    public void Awake()//added to ensure level manager runs prior to event manager
    {
       // Cursor.SetCursor( BEAVERSAM-cursor );
        currentActiveLevel = Menu;
        eventManager = Object.FindFirstObjectByType<EventManager>();// find the event manager
        addAudio = Object.FindFirstObjectByType<AddAudio>();// find the audio  adder
        alphaPOC_BossCombatController = Object.FindFirstObjectByType<AlphaPOC_BossCombatController>();// find and initalize
    }
    public void Start()
    {
        CloseAllScreens();// ensures no other active scenes at start 
        Menu.SetActive(true);// sets default starting stage
    }
    public void CloseAllScreens() //closes all levels
    {
        Level01.SetActive(false);
        Level02.SetActive(false);
        Level03.SetActive(false);
        Boss.SetActive(false);
        Tutorial.SetActive(false);
        Menu.SetActive(false);
        HUD.SetActive(false);
    }
    public void levelChange(GameObject levelToLoad) // processes level change 
    {
        CloseAllScreens();
        eventManager.TurnOnPlayer();
        if (levelToLoad != null)
        {
            levelToLoad.SetActive(true);
            currentActiveLevel = levelToLoad; // updates level tracker
        }
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
        if (levelToLoad == Level01 || levelToLoad == Level02 || levelToLoad == Level03 )
        {
            HUD.SetActive(true);
        }
        else
        {
            Cursor.visible = true; //  makes  cursor visible on stages without player controller           
           // Cursor.lockState = CursorLockMode.None;// unlocks  cursor for stags with no player  controller

            HUD.SetActive(false); // Hide HUD for Menu/Tutorial/ boss
        }
        if (addAudio != null)// sets the audio per level 
        {
            if (levelToLoad == Menu || levelToLoad == Tutorial) 
            {
                addAudio.OnMenu(); 
            }
            else if (levelToLoad == Boss) 
            {
                addAudio.OnBoss(); 
            }
            else 
            { 
                addAudio.OnStage();
            }
        }
    }
    public void LoadNextChronologicalLevel()  //  loads stages in next chronological order
    {
        if (currentActiveLevel == Level01)
        {
            levelChange(Level02);
        }
        else if (currentActiveLevel == Level02)
        {
            levelChange(Level03);
        }
        else if (currentActiveLevel == Level03)
        {
            levelChange(Boss);
        }
    }
    public void onStart() // processes level change 
    {       
        currentActiveLevel.SetActive(false);
        Level01.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = Level01;
        Cursor.visible = false;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onMenu() // processes level change 
    {   
        currentActiveLevel.SetActive(false);
        Menu.SetActive(true);
        HUD.SetActive(false);
        currentActiveLevel = Menu;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onLevel01() // processes level change 
    {
        currentActiveLevel.SetActive(false);
        Level01.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = Level01;
        Cursor.visible = false;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onLevel02() // processes level change 
    {    
        currentActiveLevel.SetActive(false);
        Level02.SetActive(true);
        HUD.SetActive(true); 
        currentActiveLevel = Level02;
        Cursor.visible = false;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onLevel03() // processes level change 
    {    
        currentActiveLevel.SetActive(false);
        Level03.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = Level03;
        Cursor.visible = false;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onTutorial() // processes level change 
    {
        currentActiveLevel.SetActive(false);
        Tutorial.SetActive(true);
        HUD.SetActive(false);
        currentActiveLevel = Tutorial;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
    public void onBoss() // processes level change 
    {  
        currentActiveLevel.SetActive(false);
        Boss.SetActive(true);
        HUD.SetActive(false);
        currentActiveLevel = Boss;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }
    }
}
