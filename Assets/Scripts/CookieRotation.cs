using UnityEngine;

public class CookieRotation : MonoBehaviour
{
   [SerializeField] public float rotationSpeed = 50f;

    void Update()
    {
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // puts rotation on cookie object 
    }
}