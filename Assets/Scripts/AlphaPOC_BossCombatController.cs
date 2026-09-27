
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;

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

public class AlphaPOC_BossCombatController : MonoBehaviour
{
    [SerializeField] private TMP_Text Patience;
    public TextMeshProUGUI combatInfoBox;
    [SerializeField] public GameObject BossImage;
    [SerializeField] public int MaxPatience;
    public int DodgeSpot=2;
    private LevelManager levelManager;
    private GameExitManager gameExitManager;
    private AddAudio addAudio;
    private EventManager eventManager;
    private bool iscombat = false;


    private void Awake()
    {
      
        levelManager = Object.FindFirstObjectByType<LevelManager>(); // initalizes
        gameExitManager = Object.FindFirstObjectByType<GameExitManager>(); // initalizes
        addAudio = Object.FindFirstObjectByType<AddAudio>(); // initalizes
        eventManager = Object.FindFirstObjectByType<EventManager>(); // initalizes
    }






    private void OnEnable()
    {
        // Reset states cleanly right when the Boss object is enabled by your LevelManager
        MaxPatience = 6;
        DodgeSpot = 0;
        iscombat = false;
        updatePatience();
    }

    // Update is called once per frame
    void Update()
    {

        bossFight();


    }

    public void bossFight()
    {
        if (MaxPatience > 0 && !iscombat)
        {
            StartCoroutine(SimulateTurnBasedCombat());

        }
    }


    IEnumerator SimulateTurnBasedCombat()
    {
        iscombat = true;

        DodgeChoice();

        yield return new WaitForSeconds(5.0f);

        CheckBossFight();

        iscombat = false;
    }

    public void CheckBossFight()
    {
        int bossattack = UnityEngine.Random.Range(1, 4);

        if (bossattack == DodgeSpot)
        {
            eventManager.ItemsCount--;

            eventManager.DisplayInfoMessage("Oh Noes Dangle Grabbier grabbed a cookie... you need all you can get doge harder");

        }
        else
        {
            eventManager.DisplayInfoMessage("YAY you Doged the grabbiegrab... you're better than DBZ Abridged Gohan, whoever that is...");

        }

         updatePatience();
    }

    public void DodgeChoice()
    {
        eventManager.DisplayInfoMessage("\u001b[38;2;255;165;0mDangle is about to grabbie grab which way do you dodge\n 1 for left, 2 for stay still 3 for right");

        if (Input.GetKeyDown(KeyCode.Alpha1)) { DodgeSpot = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { DodgeSpot = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { DodgeSpot = 3; }

     }
    public void updatePatience()
    {

        MaxPatience--;
        if (MaxPatience <= 0)
        {
            eventManager.DisplayInfoMessage("The Dangle Dragon has gotten frustraited with all your Beaverie jumping around \n it has run away with whatever cookies it could grabbie grab...");

            BossImage.SetActive(false);
            gameExitManager.Wincheck();

        }
        else { }

    }


}
