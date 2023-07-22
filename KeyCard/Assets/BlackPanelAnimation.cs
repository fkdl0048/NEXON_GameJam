using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BlackPanelAnimation : MonoBehaviour
{
    Sequence sequence;
    [SerializeField] TMP_Text text;
    [SerializeField] Image descryption;

    public void OnEnablePanel()
    {
        text.gameObject.SetActive(true);
        descryption.gameObject.SetActive(true);

        sequence = DOTween.Sequence();
        sequence
            .Append(GetComponent<SpriteRenderer>().DOFade(0.5f, 0.5f))
            .Append(descryption.DOFade(1f, 0.5f))
            .Join(text.DOFade(1f, 0.5f));
    }

    public void DisablePanel()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(GetComponent<SpriteRenderer>().DOFade(0, 0.5f))
            .Join(descryption.DOFade(0, 0.5f))
            .Join(text.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                text.gameObject.SetActive(false);
                descryption.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });
    }

    private void OnMouseDown()
    {
        CardManager.Instance.CardSelectCancle();
    }
}
