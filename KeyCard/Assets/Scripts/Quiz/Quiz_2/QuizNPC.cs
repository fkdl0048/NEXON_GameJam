using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizNPC : MonoBehaviour
{
    public ObjectOutline objectOutline;
    
    public Action<QuizNPC> OnClickAction;
    public bool IsSelected { get; set; } = false;
    
    void Awake()
    {
        objectOutline = new ObjectOutline(GetComponent<Image>().material);
        
        objectOutline.ObjectOutlineOff();
    }
    
    public void OnClick()
    {
        if (IsSelected)
        {
            objectOutline.ObjectOutlineOff();
            objectOutline.ObjectOutlineWhite();
            IsSelected = false;
        }
        else
        {
            objectOutline.ObjectOutlineOn();
            objectOutline.ObjectOutlineRed();
            IsSelected = true;
        }
        
        OnClickAction?.Invoke(this);
    }
    
    public void OnPointerEnter()
    {
        objectOutline.ObjectOutlineOn();
    }

    public void OnPointerExit()
    {
        if (!IsSelected)
        {
            objectOutline.ObjectOutlineOff();
        }
    }
}
