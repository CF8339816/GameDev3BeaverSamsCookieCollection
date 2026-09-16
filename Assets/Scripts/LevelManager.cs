using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject Level01;
    public GameObject Level02;
    public GameObject Level03;
    public GameObject BossFight;
    public GameObject Tutorial;
    public GameObject Menu;
    public GameObject Settings;
    public GameObject PauseScreen;
    private GameObject player;
    public GameObject currentActiveLevel;
    public Transform spawnLocation;
    public GameObject levelToLoad;
    //public GameObject level; 


    public void Start()
    {
        currentActiveLevel = Menu;
        // player = ServiceHub.Instance.playerController.gameObject;
    }
    public void CloseAllScreens()
    {
        Menu.SetActive(false);
        Settings.SetActive(false);
        PauseScreen.SetActive(false);
        Level01.SetActive(false);
        Level02.SetActive(false);
        Level03.SetActive(false);
        BossFight.SetActive(false);
        Tutorial.SetActive(false);

    }
    public void levelChange(GameObject levelToLoad, Transform spawnLocation)
    {
        CloseAllScreens();

        currentActiveLevel.SetActive(false);
        levelToLoad.SetActive(true);
        currentActiveLevel = levelToLoad;

        player.transform.position = spawnLocation.position;
    }

    public void LoadNextChronologicalLevel()
    {
        if (currentActiveLevel == Menu || currentActiveLevel == Tutorial)
        {
            levelChange(Level01, spawnLocation);
        }
        else if (currentActiveLevel == Level01)
        {
            levelChange(Level02, spawnLocation);
        }
        else if (currentActiveLevel == Level02)
        {
            levelChange(Level03, spawnLocation);
        }
        else if (currentActiveLevel == Level03)
        {
            levelChange(BossFight, spawnLocation);
        }
        else
        {
            levelChange(Menu, spawnLocation);
        }

    }
}
