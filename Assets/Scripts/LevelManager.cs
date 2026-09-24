using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //public Scene Level;
    //public Scene Menu;
    //public Scene Tutorial;
    public GameObject Level01;
    public GameObject Level02;
    public GameObject Level03;
    public GameObject BossStage;
    public GameObject Tutorial;
    public GameObject Menu;
    public GameObject currentActiveLevel;
    private LevelGeneration levelGeneration;
    public GameObject levelToLoad;
    private EventManager eventManager;  //added to ensure level manager can find the event manager to tell it when to initalize stages

    public GameObject HUD;

    public void Awake()//added to ensure level manager runs prior to event manager
    {

        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //Level01= levelGeneration.GenerateLevel();
        //Level02= levelGeneration.GenerateLevel();
        //Level03 = levelGeneration.GenerateLevel();



        currentActiveLevel = Menu;
        eventManager = Object.FindFirstObjectByType<EventManager>();// find the event manager
    }

    public void Start()
    {
        CloseAllScreens();// ensures no other active scenes at start 
                          //Menu.SetActive(true); // ensures level  1  initalized
        Menu.SetActive(true);
        // currentActiveLevel = Level01;// sets default starting stage

    }
    public void CloseAllScreens() //closes all levels
    {
        
        Level01.SetActive(false);
        Level02.SetActive(false);
        Level03.SetActive(false);
       BossStage.SetActive(false);
        Tutorial.SetActive(false);
        Menu.SetActive(false);
        HUD.SetActive(false);
    }
    public void levelChange(GameObject levelToLoad) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        levelToLoad.SetActive(true);
        currentActiveLevel = levelToLoad;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
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

    }

    public void onStart(GameObject Level01) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Level01.SetActive(true);
        currentActiveLevel = Level01;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }

    public void onMenu(GameObject Menu) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Menu.SetActive(true);
        currentActiveLevel = Menu;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }
    public void onLevel01(GameObject Level01) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Level01.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = Level01;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }

    public void onLevel02(GameObject Level02) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Level02.SetActive(true);
        HUD.SetActive(true); 
        currentActiveLevel = Level02;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }

    public void onLevel03(GameObject Level03) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Level03.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = Level03;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }

    public void onTutorial(GameObject Tutorial) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        Tutorial.SetActive(true);
        currentActiveLevel = Tutorial;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }


    public void onBoss(GameObject BossStage) // processes level change 
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        BossStage.SetActive(true);
        HUD.SetActive(true);
        currentActiveLevel = BossStage;


        if (eventManager != null)// tells event manager to load new level 
        {
            eventManager.OnLevelChange(currentActiveLevel);
        }

    }





}
