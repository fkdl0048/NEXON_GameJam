using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    private bool isClicked = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !isClicked)
        {
            isClicked = true;
            fadeController.FadeIn();
            
            DOVirtual.DelayedCall(1, () =>
            {
                AsyncSceneLoader.LoadScene("Dialogue");
            });
        }
    }
}
