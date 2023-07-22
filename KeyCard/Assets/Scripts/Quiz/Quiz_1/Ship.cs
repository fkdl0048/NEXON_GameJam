using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour
{
    [SerializeField] private RectTransform targetpos1;
    [SerializeField] private RectTransform targetpos2;
    [SerializeField] private Transform itempos1;
    [SerializeField] private Transform itempos2;

    public Action OnClickAction;
    public bool IsMoving { get; private set; } = false;
    public bool IsLeft { get; private set; } = false;

    private bool isItem1Full = false;
    private bool isItem2Full = false;
    
    public void OnClick()
    {
        if (IsMoving)
        {
            return;
        }

        if (!NeedHuman())
        {
            return;
        }

        IsMoving = true;

        if (IsLeft)
        {
            transform.DOMove(targetpos1.position, 2).OnComplete(() =>
            {
                IsMoving = false;
                IsLeft = false;
                AllBaggageUnload();
            });
        }
        else
        {
            transform.DOMove(targetpos2.position, 2).OnComplete(() =>
            {
                IsMoving = false;
                IsLeft = true;
                AllBaggageUnload();
            });
        }
        
        OnClickAction?.Invoke();
    }
    
    public bool AddItem(Transform item)
    {
        if (isItem1Full && isItem2Full)
        {
            return false;
        }

        if (item.GetComponent<Baggage>().IsBoard)
        {
            return false;
        }
        
        if (!isItem1Full)
        {
            item.position = itempos1.position;
            item.SetParent(itempos1);
            isItem1Full = true;
        }
        else
        {
            item.position = itempos2.position;
            item.SetParent(itempos2);
            isItem2Full = true;
        }

        return true;
    }
    
    public void RemoveItem(Transform item)
    {
        if (item.parent == itempos1)
        {
            isItem1Full = false;
        }
        else
        {
            isItem2Full = false;
        }
    }

    private void AllBaggageUnload()
    {
        if (itempos1.childCount > 0)
        {
            itempos1.GetChild(0).GetComponent<Baggage>().Export();
        }

        if (itempos2.childCount > 0)
        {
            itempos2.GetChild(0).GetComponent<Baggage>().Export();
        }
    }

    private bool NeedHuman()
    {
        bool isNeedHuman = false;
        
        if (itempos1.childCount > 0)
        {
            if (itempos1.GetChild(0).GetComponent<Baggage>().BaggageType == BaggageType.Police ||
                itempos1.GetChild(0).GetComponent<Baggage>().BaggageType == BaggageType.Prisoner)
            {
                isNeedHuman = true;
            }
        }
        
        if (itempos2.childCount > 0)
        {
            if (itempos2.GetChild(0).GetComponent<Baggage>().BaggageType == BaggageType.Police ||
                itempos2.GetChild(0).GetComponent<Baggage>().BaggageType == BaggageType.Prisoner)
            {
                isNeedHuman = true;
            }
        }

        if (isNeedHuman)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        transform.DOMove(targetpos2.position, 2).OnComplete(() =>
        {
            IsMoving = false;
            IsLeft = true;
        });
    }
    
}
