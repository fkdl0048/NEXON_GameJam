using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class CreditImage : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    private bool isClicked = false;

    void Start()
    {
        GameManager.Instance.GameState = GameState.Credit;
        fadeController.FadeOut();
    }

    void Update()
    {
        if (Input.anyKey && !isClicked)
        {
            isClicked = true;
            fadeController.FadeIn();

            DOVirtual.DelayedCall(1, () =>
            {
                AsyncSceneLoader.LoadScene("Title");
            });
        }
    }
}
