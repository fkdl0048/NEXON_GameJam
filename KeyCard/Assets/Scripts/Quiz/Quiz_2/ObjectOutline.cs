using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOutline
{
    private Material material;

    public ObjectOutline(Material material)
    {
        this.material = material;
        
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
    
    public void ObjectOutlineRed()
    {
        material.SetColor("_OutlineColor", new Color(0.69f, 0.23f, 0.28f));
    }
    
    public void ObjectOutlineWhite()
    {
        material.SetColor("_OutlineColor", Color.white);
    }
}
