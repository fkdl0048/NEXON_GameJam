using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelsQuiz : MonoBehaviour
{
    [SerializeField] private QuizNPC[] quizNPCs;
    [SerializeField] private QuizController quizController;

    private void Start()
    {
        foreach (var quizNpC in quizNPCs)
        {
            quizNpC.OnClickAction += CheckOneSelect;
            quizNpC.objectOutline.ObjectOutlineWhite(); 
        }
    }

    private void CheckOneSelect(QuizNPC quizNPC)
    {
        foreach (var npc in quizNPCs)
        {
            if (npc != quizNPC)
            {
                npc.IsSelected = false;
                npc.objectOutline.ObjectOutlineOff();
            }
            else
            {
                npc.IsSelected = true;
                npc.objectOutline.ObjectOutlineOn();
                npc.objectOutline.ObjectOutlineWhite();
            }
        }
    }

    public void OnClick()
    {
        if (quizNPCs[1].IsSelected)
        {
            GameManager.Instance.GameState = GameState.Quiz_3;
            quizController.DialogueScecne();
        }
        else
        {
            AsyncSceneLoader.LoadScene("Quiz");
        }
        // 정답 처리
    }
}
