using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            fadeController.FadeIn();
            
            DOVirtual.DelayedCall(1, () => { SceneManager.LoadScene("Dialogue"); });    
            
            //SceneManager.LoadScene("Dialogue");
        }
    }
}
