using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Material material;
    void Start()
    {
        material = GetComponentInChildren<Image>().material;

        if (material == null)
        {
            Debug.LogError("Material is null");
        }
            
        
        material.SetFloat("_FadeAmount", 1f);
    }

    public void FadeIn()
    {
        material.SetFloat("_FadeAmount", 1f);
        StartCoroutine(FadeInCo());
    }
    
    public void FadeOut()
    {
        material.SetFloat("_FadeAmount", -0.1f);
        StartCoroutine(FadeOutCo());
    }
    
    IEnumerator FadeInCo()
    {
        float fadeAmount = 1f;
        while (fadeAmount >= -0.1f)
        {
            fadeAmount -= Time.deltaTime;
            material.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }
    }
    
    IEnumerator FadeOutCo()
    {
        float fadeAmount = -0.1f;
        while (fadeAmount <= 1f)
        {
            fadeAmount += Time.deltaTime * 0.5f;
            material.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }
    }
}
