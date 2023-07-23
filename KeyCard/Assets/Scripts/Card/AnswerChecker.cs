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
        foreach (var player in answers)
        {
            if (!player.isInput)
                return;
        }
        
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
            GameManager.Instance.GameState = GameState.HappyEnddingDialogue;
            quizController.DialogueScecne();
        }
        else if (fake)
        {
            GameManager.Instance.GameState = GameState.BadEnddingDialogue;
            quizController.DialogueScecne();
        }
        else
        {
            playerAnswer.Clear();

            foreach (var item in answers)
            {
                item.ClearAnswer();
            }

            //quizController.DialogueScecne();

            // �絵��~
        }
    }
}
