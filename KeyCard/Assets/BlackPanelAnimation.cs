using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackPanelAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Image>().DOFade(0.5f, 1f);
    }

    private void OnDisable()
    {
        var alpha = GetComponent<Image>().color;
        alpha.a = 0;
        GetComponent<Image>().color = alpha;
    }
}
