using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer forntSpriteRenderer;
    [SerializeField] SpriteRenderer backSpriteRenderer;
    [SerializeField] SpriteRenderer cardImage;

    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite blank;

    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text keyTMP;

    public PRS originPRS;
    public Item item;

    bool isFront;

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            nameTMP.text = item.name;
            cardImage.sprite = item.image;
        }
        else
        {
            forntSpriteRenderer.sprite = cardBack;
            backSpriteRenderer.sprite = cardFront;
            cardImage.sprite = blank;
            nameTMP.text = "";

        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public IEnumerator MoveTransformCoroutine(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        keyTMP.text = "";
        if (useDotween)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOMove(prs.pos, dotweenTime))
                .Join(transform.DORotateQuaternion(prs.rot, dotweenTime))
                .Join(transform.DOScale(prs.scale, dotweenTime))
                .Join(GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
                .Join(nameTMP.GetComponent<TMP_Text>().DOFade(0, dotweenTime));
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }

        yield return new WaitForSeconds(dotweenTime);
    }

    public void SetKey(string keycode)
    {
        keyTMP.text = keycode;
    }

    void OnDisable()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        nameTMP.GetComponent<TMP_Text>().color = Color.white;
    }

    void OnMouseOver()
    {
        if (isFront)
            CardManager.Instance.CardMouseOver(this);
    }

    void OnMouseDown()
    {
        CardManager.Instance.CardMouseDown(this);
    }

    void OnMouseExit()
    {
        if (isFront)
            CardManager.Instance.CardMouseExit(this);
    }
}
