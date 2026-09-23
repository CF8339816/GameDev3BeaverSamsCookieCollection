using UnityEngine;
using System.Collections;


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

public class IconVisibility : MonoBehaviour
{
    [SerializeField] public GameObject nomIcon;
    [SerializeField] public GameObject idleIcon;

    [SerializeField] public float visibleTime=3f;
    // Call this function to trigger the 3-second visibility
    public void FlashVisible()
    {
        StartCoroutine(VisibilityRoutine());
    }

    private IEnumerator VisibilityRoutine()
    {
        idleIcon.SetActive(false);
        nomIcon.SetActive(true);

        yield return new WaitForSeconds(visibleTime);

        
        nomIcon.SetActive(false);
        idleIcon.SetActive(true);
    }

}
