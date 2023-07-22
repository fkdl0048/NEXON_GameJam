using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baggage : MonoBehaviour
{
    [SerializeField] private Ship ship;
    
    [SerializeField] private Transform BaggagePos1;
    [SerializeField] private Transform BaggagePos2;

    public bool IsLeft { get; set; } = false;
    public bool IsBoard { get; set; } = false;

    public void OnClick()
    {
        if (!ship.AddItem(transform) && !IsBoard)
        {
            if (IsLeft)
            {
                transform.position = BaggagePos1.position;
                transform.SetParent(BaggagePos1);
            }
            else
            {
                transform.position = BaggagePos2.position;
                transform.SetParent(BaggagePos2);
            }
        }
        else
        {
            IsBoard = true;
        }
    }
}
