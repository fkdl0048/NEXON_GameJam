using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizNPC : MonoBehaviour
{
    private ObjectOutline objectOutline;
    
    public bool IsSelected { get; set; } = false;
    
    void Start()
    {
        objectOutline = new ObjectOutline(GetComponent<Image>().material);
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
