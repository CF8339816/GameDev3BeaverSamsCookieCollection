using UnityEngine;
using System.Collections;
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
