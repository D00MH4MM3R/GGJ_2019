using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleAndLoop : MonoBehaviour
{
    Vector3 originalScale;// = this.gameObject.transform.localScale;
    Vector3 destinationScale;//= new Vector3(0.0f, 0.0f, 0.0f);
    
    float currentTime = 0.0f;
    float time = 10.0f;
    
    void Awake()
    {
        originalScale = this.gameObject.transform.localScale;
        destinationScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
         this.gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
         
         currentTime += Time.deltaTime;
         
         if (transform.localScale == destinationScale)
         {
             SceneManager.LoadScene("Splash");
         }
    }
}
