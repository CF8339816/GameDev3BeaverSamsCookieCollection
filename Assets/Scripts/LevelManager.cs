using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject Level01;
    public GameObject Level02;
    public GameObject Level03;
    //public GameObject BossFight;
    //public GameObject Tutorial;
    //public GameObject Menu;
    public GameObject currentActiveLevel;

    public GameObject levelToLoad;
    private EventManager eventManager;  //added to ensure level manager can find the event manager to tell it when to initalize stages


    public void Awake()//added to ensure level manager runs prior to event manager
    {
        //currentActiveLevel = Menu;//ensures level 1 initalized before event manager stsart to remove nulling issue causing the missync issue in the level collection  counter
        currentActiveLevel = Level01;
        eventManager = Object.FindFirstObjectByType<EventManager>();// find the event manager
    }

    public void Start()
    {
        CloseAllScreens();// ensures no other active scenes at start 
                          //Menu.SetActive(true); // ensures level  1  initalized
        Level01.SetActive(true);
        // currentActiveLevel = Level01;// sets default starting stage

    }
    public void CloseAllScreens() //closes all levels
    {
        
        Level01.SetActive(false);
        Level02.SetActive(false);
        Level03.SetActive(false);
        //BossFight.SetActive(false);
        //Tutorial.SetActive(false);
        //Menu.SetActive(false);
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
}
