
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
    public TextMeshProUGUI textPatience;
    [SerializeField] private TMP_Text BossFightInfoBox;
    public TextMeshProUGUI textBossFightInfoBox;
    [SerializeField] public GameObject BossImage;
    [SerializeField] public GameObject Menu;
    [SerializeField] public GameObject Quit;
    [SerializeField] public GameObject Restart;
    [SerializeField] public int MaxPatience=4;
   
    public int DodgeSpot=2;
    private LevelManager levelManager;
    private GameExitManager gameExitManager;
    private AddAudio addAudio;
    private EventManager eventManager;
    private bool iscombat = false;

    private Coroutine activeBossTextTimer; //defines timer coroutine for message duration

    private IEnumerator ClearTextBoxAfterDelay() //setting up diisplay timer for info box messages
    {
        yield return new WaitForSeconds(3);  // allows for time delay set in seconds
        textBossFightInfoBox.text = ""; //Sets cleared message
    }
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
        MaxPatience = 4;
        DodgeSpot = 2;
        iscombat = false;
     }

    private void Start()
    {      
        BossImage.SetActive(true);
        Quit.SetActive(false);
        Restart.SetActive(false);
        Menu.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        bossFight();
    
        textPatience.text = "Dangle's Patience " + MaxPatience.ToString();
    }
    public void bossFight()
    {
        if (MaxPatience > 0 && !iscombat)
        {
            StartCoroutine(SimulateTurnBasedCombat());
        }
    }
    public void DisplayBossFightMessage(string message)// formats info box messages to utalize display clear timer instead of being on screen dynamically
    {
        textBossFightInfoBox.text = message; //defines new message variavle name
        if (activeBossTextTimer != null) // stops any currently running timer  upon new one started
        {
            StopCoroutine(activeBossTextTimer);
        }
        activeBossTextTimer = StartCoroutine(ClearTextBoxAfterDelay()); //starts newly defined timer (currently 4 sec)
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
            DisplayBossFightMessage("Oh Noes Dangle Grabbier grabbed a cookie... you need all you can get doge harder");
        }
        else
        {
            DisplayBossFightMessage("YAY you Doged the grabbiegrab... you're better than DBZ Abridged Gohan, whoever that is...");
        }
         updatePatience();
    }
    public void DodgeChoice()
    {
        DisplayBossFightMessage("\u001b[38;2;255;165;0mDangle is about to grabbie grab which way do you dodge\n 1 for left, 2 for stay still 3 for right");
        if (Input.GetKeyDown(KeyCode.Alpha1)) { DodgeSpot = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { DodgeSpot = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { DodgeSpot = 3; }
     }
    public void updatePatience()
    {
        MaxPatience--;
        if (MaxPatience < 1)
        {
            DisplayBossFightMessage("The Dangle Dragon has gotten frustraited with all your Beaverie jumping around \n it has run away with whatever cookies it could grabbie grab...");
            BossImage.SetActive(false);
            Quit.SetActive(true);
            Restart.SetActive(true);
            Menu.SetActive(true);
            
        }
        else { }
    }

}
