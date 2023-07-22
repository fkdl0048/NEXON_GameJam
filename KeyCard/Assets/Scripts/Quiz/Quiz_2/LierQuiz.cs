using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LierQuiz : MonoBehaviour
{
    [SerializeField] private QuizNPC[] quizNPCs;
    [SerializeField] private QuizController quizController;
    
    public void OnClick()
    {
        int count = 0;
        foreach (var quizNPC in quizNPCs)
        {
            if (quizNPC.IsSelected)
            {
                count++;
            }
        }

        if (count == quizNPCs.Length)
        {
            quizController.DialogueScecne();
        }
        else
        {
            AsyncSceneLoader.LoadScene("Quiz");   
        }
    }
    
}
