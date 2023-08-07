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
        GameManager.Instance.GameState = GameState.Title;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Z)
            && !isClicked)
        {
            DialougueScene();
        }
    }

    public void DialougueScene()
    {
        isClicked = true;
        fadeController.FadeIn();

        DOVirtual.DelayedCall(1, () =>
        {
            AsyncSceneLoader.LoadScene("Dialogue");
        });
    }

    public void CreditScene()
    {
        isClicked = true;
        fadeController.FadeIn();

        DOVirtual.DelayedCall(1, () =>
        {
            AsyncSceneLoader.LoadScene("Credit");
        });
    }
}
