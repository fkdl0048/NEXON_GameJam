using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    Card answerCard;
    int prevNum;
    public bool isInput;

    public void InputAnswer(Card card, int prevNum)
    {
        answerCard = card;
        isInput = true;
        this.prevNum = prevNum;
    }

    public int GetAnswer()
    {
        return answerCard.item.id;
    }

    public void ClearAnswer()
    {
        StartCoroutine(CardManager.Instance.CardBackCoroutine(answerCard, prevNum));
        answerCard = null;
        isInput = false;
    }
}
