using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BaggageQuiz : MonoBehaviour
{
    [SerializeField] private Baggage[] baggages;
    [SerializeField] private Ship ship;
    // Start is called before the first frame update
    void Start()
    {
        ship.OnClickAction += () =>
        {
            DOVirtual.DelayedCall(2.2f, OnValidate);
        };
    }

    private void OnValidate()
    {
        int leftPoliceCount = 0;
        int leftPrisonerCount = 0;
        int leftGoldCount = 0;
        int rightPoliceCount = 0;
        int rightPrisonerCount = 0;
        int rightGoldCount = 0;
        
        foreach (var baggage in baggages)
        {
            if (baggage.BaggageType == BaggageType.Prisoner)
            {
                if (baggage.IsRight)
                {
                    rightPrisonerCount++;
                }
                else
                {
                    leftPrisonerCount++;
                }
            }
            
            if (baggage.BaggageType == BaggageType.Police)
            {
                if (baggage.IsRight)
                {
                    rightPoliceCount++;
                }
                else
                {
                    leftPoliceCount++;
                }
            }
            
            if (baggage.BaggageType == BaggageType.Gold)
            {
                if (baggage.IsRight)
                {
                    rightGoldCount++;
                }
                else
                {
                    leftGoldCount++;
                }
            }
        }
        
        if ((leftPrisonerCount >= leftPoliceCount + 2) || (rightPrisonerCount >= rightPoliceCount + 2))
        {
            AsyncSceneLoader.LoadScene("Quiz");
            //Reset();
        }
        
        if (leftPoliceCount == 0 && leftGoldCount >= 1 && leftPrisonerCount >= 1)
        {
            AsyncSceneLoader.LoadScene("Quiz");
            //Reset();
        }
        if (rightPoliceCount == 0 && rightGoldCount >= 1 && rightPrisonerCount >= 1)
        {
            AsyncSceneLoader.LoadScene("Quiz");
            //Reset();
        }
        
        if (rightPoliceCount == 2 && rightGoldCount == 2 && rightPrisonerCount == 3)
        {
            GameManager.Instance.GameState = GameState.Dialogue_2;
            AsyncSceneLoader.LoadScene("Dialogue");
            //Reset();
        }
    }

    private void Reset()
    {
        foreach (var baggage in baggages)
        {
            baggage.Reset();
        }
        ship.Reset();
    }
}
