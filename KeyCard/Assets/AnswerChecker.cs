using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AnswerChecker : Singleton<AnswerChecker>
{
    [SerializeField] Answer[] answers;

    List<int> playerAnswer;
    List<int> realAnswer;
    List<int> fakeAnswer;

    private void Start()
    {
        playerAnswer = new List<int>();
        realAnswer = new List<int>() { 1, 3, 4 };
        fakeAnswer = new List<int>() { 1, 5, 6 };
    }

    public void AnswerCheck()
    {
        foreach (var item in answers)
        {
            playerAnswer.Add(item.GetAnswer());
        }

        var real = playerAnswer.SequenceEqual(realAnswer);
        var fake = playerAnswer.SequenceEqual(fakeAnswer);

        if (real)
        {
            Debug.Log("진엔딩");
            // 진엔딩
        }
        else if (fake)
        {
            Debug.Log("가짜엔딩");
            // 가짜엔딩
        }
        else
        {
            Debug.Log("실패");

            playerAnswer.Clear();

            foreach (var item in answers)
            {
                item.ClearAnswer();
            }
            // 재도전~
        }
    }
}
