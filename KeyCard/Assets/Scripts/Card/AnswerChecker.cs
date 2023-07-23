using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AnswerChecker : Singleton<AnswerChecker>
{
    [SerializeField] Answer[] answers;
    [SerializeField] QuizController quizController;

    List<int> playerAnswer;
    List<int> realAnswer;
    List<int> fakeAnswer;

    public bool isDialogueScene;

    private void Start()
    {
        playerAnswer = new List<int>();
        realAnswer = new List<int>() { 1, 3, 4 };
        fakeAnswer = new List<int>() { 1, 5, 6 };

        if(isDialogueScene)
            StartCoroutine(GetQuizController());
    }

    IEnumerator GetQuizController()
    {
        yield return new WaitForEndOfFrame();
        quizController = GameObject.FindWithTag("Quiz").GetComponent<QuizController>();
    }

    public void AnswerCheck()
    {
        if (playerAnswer == null)
        {
            return;
        }

        foreach (var item in answers)
        {
            playerAnswer.Add(item.GetAnswer());
        }

        var real = playerAnswer.SequenceEqual(realAnswer);
        var fake = playerAnswer.SequenceEqual(fakeAnswer);

        if (real)
        {
            Debug.Log("진 엔딩");
            quizController.DialogueScecne();
        }
        else if (fake)
        {
            Debug.Log("가짜엔딩");
            quizController.DialogueScecne();
        }
        else
        {
            Debug.Log("실패");

            playerAnswer.Clear();

            foreach (var item in answers)
            {
                item.ClearAnswer();
            }

            quizController.DialogueScecne();

            // 재도전~
        }
    }
}
