using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class VolumeManager : Singleton<VolumeManager>
{
    protected VolumeManager () { }
    
    private Volume _volume;
    private Vignette _Vignette;
    
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.0f;
    public float pulsateDuration = 1.0f;

    private bool isPulsating = false;

    private void Awake()
    {
        _volume = GetComponent<Volume>();

       // _volume.profile.TryGet(out _Vignette);

        //StartCoroutine(StartVigette());
    }

    private IEnumerator StartVigette()
    {
        while (true)
        {
            // 효과를 줄이는 단계
            isPulsating = true;
            float elapsedTime = 0f;
            while (elapsedTime < pulsateDuration)
            {
                float t = elapsedTime / pulsateDuration;
                //float intensity = Mathf.Lerp();
                //_Vignette.intensity = new ClampedFloatParameter(maxIntensity, minIntensity, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 효과를 다시 키우는 단계
            isPulsating = false;
            elapsedTime = 0f;
            while (elapsedTime < pulsateDuration)
            {
                float t = elapsedTime / pulsateDuration;
                //float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
                //_Vignette.intensity = new ClampedFloatParameter(minIntensity, maxIntensity, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
    
    public void StartVignette()
    {
        if (isPulsating)
        {
            return;
        }

        //_Vignette.color = new ColorParameter(Color.red);

    }
    
    
}
