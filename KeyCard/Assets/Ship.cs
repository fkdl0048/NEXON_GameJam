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
    
    private bool isMoving = false;
    private bool isLeft = false;
    
    private bool isItem1Full = false;
    private bool isItem2Full = false;
    
    public void OnClick()
    {
        if (isMoving)
        {
            return;
        }
        
        isMoving = true;
        if (isLeft)
        {
            transform.DOMove(targetpos1.position, 2).OnComplete(() =>
            {
                isMoving = false;
                isLeft = false;
            });
        }
        else
        {
            transform.DOMove(targetpos2.position, 2).OnComplete(() =>
            {
                isMoving = false;
                isLeft = true;
            });
        }
    }
    
    public bool AddItem(Transform item)
    {
        if (isItem1Full && isItem2Full)
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

}
