using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOutline : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        material = GetComponent<Image>().material;
        
        if (material == null)
        {
            Debug.LogError("Material is null");
        }
    }
    
    public void ObjectOutlineOn()
    {
        material.SetFloat("_OutlineAlpha", 1);
    }
    
    public void ObjectOutlineOff()
    {
        material.SetFloat("_OutlineAlpha", 0);
    }
}
