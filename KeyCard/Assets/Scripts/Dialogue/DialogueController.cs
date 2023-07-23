using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueController : Singleton<DialogueController>
{
    [SerializeField] private FadeController fadeController;
    [SerializeField] private GameObject[] dialogueObjects;


    int count = 2;
    bool first;
    Button button;

    void Start()
    {
        SceneManager.LoadScene("DialougeCard", LoadSceneMode.Additive);
        StartCoroutine(GetCardInteractive());
        fadeController.FadeOut();

        switch (GameManager.Instance.GameState)
        {
            case GameState.Title:
                dialogueObjects[0].SetActive(true);
                GameManager.Instance.GameState = GameState.Dialogue_1;
                break;
            case GameState.Quiz_1:
                dialogueObjects[1].SetActive(true);
                GameManager.Instance.GameState = GameState.Dialogue_2;
                break;
            case GameState.Quiz_2:
                dialogueObjects[2].SetActive(true);
                GameManager.Instance.GameState = GameState.Dialogue_3;
                break;
            case GameState.Quiz_3:
                dialogueObjects[3].SetActive(true);
                GameManager.Instance.GameState = GameState.Dialogue_4;
                break;
        }
    }

    public void QuizScene()
    {
        fadeController.FadeIn();
        DOVirtual.DelayedCall(1, () =>
        {
            AsyncSceneLoader.LoadScene("Quiz");
        });
    }
    IEnumerator GetCardInteractive()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        button = GameObject.FindWithTag("DeckOpenButton").GetComponent<Button>();
    }


    public void PlusCard()
    {
        if (!first)
        {

            button.interactable = true;
            return;
        }
        if (count < 6)
        {
            for (int i = 0; i < 2; i++)
                TurnManager.OnAddCard?.Invoke(true);
        }
    }
}
