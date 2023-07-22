using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaggageType
{
    Prisoner,
    Police,
    Gold
}

public class Baggage : MonoBehaviour
{
    [field: SerializeField]
    public BaggageType BaggageType { get; private set; }

    [SerializeField] private Ship ship;
    
    [SerializeField] private Transform BaggagePos1;
    [SerializeField] private Transform BaggagePos2;

    public bool IsRight { get; set; } = false;
    public bool IsBoard { get; set; } = false;

    public void OnClick()
    {
        if (ship.IsMoving)
        {
            return;
        }
        
        if (ship.IsLeft != IsRight)
        {
            return;
        }

        if (ship.AddItem(transform) && !IsBoard)
        {
            IsBoard = true;
        }
        else
        {
            Unload();
        }
    }

    private void Unload()
    {
        if (IsBoard)
        {
            ship.RemoveItem(transform);
            
            if (!IsRight)
            {
                transform.position = BaggagePos1.position;
                transform.SetParent(BaggagePos1);
                IsRight = false;
            }
            else
            {
                transform.position = BaggagePos2.position;
                transform.SetParent(BaggagePos2);
                IsRight = true;
            }
            
            IsBoard = false;
        }
    }

    public void Export()
    {
        if (IsBoard)
        {
            ship.RemoveItem(transform);
            
            if (!IsRight)
            {   
                transform.position = BaggagePos2.position;
                transform.SetParent(BaggagePos2);
                IsRight = true;
            }
            else
            {
                transform.position = BaggagePos1.position;
                transform.SetParent(BaggagePos1);
                IsRight = false;
            }
            
            IsBoard = false;
        }
    }

    public void Reset()
    {
        IsRight = false;
        IsBoard = false;
        transform.position = BaggagePos1.position;
        transform.SetParent(BaggagePos1);
    }
}
