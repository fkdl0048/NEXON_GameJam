using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizController : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    [SerializeField] private GameObject[] quizObjects;

    void Start()
    {
        fadeController.FadeOut();
        //SceneManager.LoadScene("Quiz4", LoadSceneMode.Additive);

        switch (GameManager.Instance.GameState)
        {
            case GameState.Dialogue_1:
                quizObjects[0].SetActive(true);
                break;
            case GameState.Dialogue_2:
                quizObjects[1].SetActive(true);
                break;
            case GameState.Dialogue_3:
                quizObjects[2].SetActive(true);
                break;
            case GameState.Dialogue_4:
                SceneManager.LoadScene("Develop", LoadSceneMode.Additive);
                GameManager.Instance.GameState = GameState.Quiz_4; // 수정해야함
                break;
        }
    }

    public void DialogueScecne()
    {
        fadeController.FadeIn();

        DOVirtual.DelayedCall(1, () => { AsyncSceneLoader.LoadScene("Dialogue"); });
    }
}
