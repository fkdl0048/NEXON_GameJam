using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class CardManager : Singleton<CardManager>
{
    [Header("오브젝트")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject blackPanel;

    [Header("트랜스 폼")]
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardUseTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] Transform cardsTransform;
    [SerializeField] Transform cardSelectTransform;
    [SerializeField] Transform cardHandTransform;

    [Header("그 외")]
    [SerializeField] TMP_Text descryptionText;
    [SerializeField] ECardState eCardState;
    [SerializeField] ItemSO itemSO;
    [SerializeField] int cardNumber;
    enum ECardState { Loading, CanUseCard, ActivatingCard, Noting }

    public List<Card> myCards;
    public List<Card> otherCards;
    List<Item> itemBuffer;

    Card selectCard;
    Card selectDragCard;

    int prevCardNumber = -1;
    bool isSelected;
    bool isCardAppearance;
    bool isCardDrag;
    bool mouseExitCoolTime;

    public bool isCardActivating;
    public bool canDrag;
    public bool canAnswer;
    GameObject answerObject;

    WaitForSeconds delay = new WaitForSeconds(0.5f);
    WaitForSeconds delay01 = new WaitForSeconds(0.1f);

    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // 뽑고 삭제
        return item;
    }

    // 모든 카드의 정보를 itemBuffer에 입력하고 랜덤으로 섞기
    private void SetupItemBuffer()
    {
        itemBuffer = new List<Item>(cardNumber); // 미리 캐퍼시티 100으로 용량 설정해주면 메모리 효율적으로 사용
        for (int i = 0; i < cardNumber; i++)
        {
            Item item = itemSO.items[i];
            //for (int j = 0; j < item.percent; j++)
            itemBuffer.Add(item);
        }

        /*        for (int i = 0; i < itemBuffer.Count; i++)
                {
                    int rand = Random.Range(i, itemBuffer.Count);
                    Item temp = itemBuffer[i];
                    itemBuffer[i] = itemBuffer[rand];
                    itemBuffer[rand] = temp;
                }*/
    }

    void Start()
    {
        DrawManager.OnAddCard += AddCard;
        SetupItemBuffer();
    }

    void OnDestroy()
    {
        DrawManager.OnAddCard -= AddCard;
    }

    void AddCard(bool isMine)
    {
        //var cardGeneratePositon = cardsParentTransform.position + Vector3.up * -4.5f;

        var cardObject = Instantiate(cardPrefab, cardSpawnTrasnform.position, Util.QI);
        cardObject.transform.position = cardSpawnTrasnform.position;
        cardObject.transform.parent = cardsTransform;

        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(isMine);
        StartCoroutine(CardAlignment());
    }

    void SetOriginOrder(bool isMine)
    {
        int count = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void Update()
    {
        SetEcardState();

        if (isCardDrag)
            CardDrag();

        DetectedAnswer();
    }

    void DetectedAnswer()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Util.MousePos, Vector3.forward);
        canAnswer = Array.Exists(hits, x => x.collider.gameObject.tag == "Answer");
        if(canAnswer)
            answerObject = Array.Find(hits, x => x.collider.gameObject.tag == "Answer").transform.gameObject;
    }

    void CardDrag()
    {
        selectDragCard.MoveTransform(new PRS(Util.MousePos, Util.QI, Vector3.one * 0.15f), false);
    }

    public void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -2f, -1f);
            card.MoveTransform(new PRS(enlargePos, Util.QI, Vector3.one * 0.11f), true, 0.15f);
            OtherCardsMove(card, true);
            isSelected = isEnlarge;
        }
        else
        {
            card.MoveTransform(card.originPRS, true, 0.15f);
            OtherCardsMove(card, false);
            isSelected = isEnlarge;
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void OtherCardsMove(Card card, bool isMove)
    {

        if (isSelected == isMove)
            return;

        int index = myCards.FindIndex(x => x == card);
        if (isMove)
        {
            if (index == myCards.Count - 1)
            {
                for (int i = 0; i < index; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * -0.5f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
            else if (index == 0)
            {
                for (int i = 1; i < myCards.Count; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * 0.5f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
            else
            {
                for (int i = index + 1; i < myCards.Count; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * 0.5f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }

                for (int i = index - 1; i >= 0; i--)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * -0.5f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
        }
        else
        {
            for (int i = 0; i < myCards.Count; i++)
            {
                myCards[i].MoveTransform(myCards[i].originPRS, true, 0.15f);
            }
        }

    }

    IEnumerator CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(cardLeft, cardRight, myCards.Count, 0.5f, Vector3.one * 0.1f);

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.35f);
        }
        yield return null;
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Util.QI;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void CardSelectCancle()
    {
        StartCoroutine(CardSelectAnimation(selectCard));
    }

    IEnumerator CardSelectAnimation(Card card)
    {
        isCardActivating = true;

        if (selectCard == null)
        {
            // 새로 선택
            selectCard = card;
            prevCardNumber = myCards.FindIndex(x => x == card);
            blackPanel.SetActive(true);
            blackPanel.GetComponent<BlackPanelAnimation>().OnEnablePanel();

            card.MouseBlock(true);
            card.MoveTransform(new PRS(cardSelectTransform.position, Util.QI, Vector3.one * 0.15f), true, 0.5f);
            myCards.Remove(card);
            StartCoroutine(CardAlignment());
            descryptionText.text = card.item.descryption;

            yield return delay;
            isCardActivating = false;
            // 설명 텍스트 변경
        }
        else
        {
            if (card == selectCard)
            {
                // 취소
                blackPanel.GetComponent<BlackPanelAnimation>().DisablePanel();
                myCards.Insert(prevCardNumber, selectCard);
                SetOriginOrder(selectCard);
                StartCoroutine(CardAlignment());
                selectCard = null;
                //descryptionText.text = "";

                yield return delay;
                isCardActivating = false;
                card.MouseBlock(false);
            }
            else
            {
                myCards.Insert(prevCardNumber, selectCard);
                prevCardNumber = myCards.FindIndex(x => x == card);
                myCards.Remove(card);
                SetOriginOrder(selectCard);
                StartCoroutine(CardAlignment());
                card.MouseBlock(true);

                card.MoveTransform(new PRS(cardSelectTransform.position, Util.QI, Vector3.one * 0.15f), true, 0.5f);
                descryptionText.text = card.item.descryption;

                yield return delay;
                selectCard.MouseBlock(false);
                selectCard = card;
                isCardActivating = false;
            }
        }
    }

    public void TakeOutControlCoroutine()
    {
        if (eCardState == ECardState.Loading)
            return;

        StartCoroutine(TakeOutCard());
    }

    public IEnumerator TakeOutCard()
    {
        if (isCardAppearance)
        {
            for (int i = 0; i < myCards.Count; i++)
            {
                Card item = myCards[i];
                item.SetOparcity(100);
            }
            cardsTransform.parent.DOMoveY(-5f, 0.5f);
        }
        else
        {
            var tween = cardsTransform.parent.DOMoveY(-9f, 0.5f);
            yield return tween.WaitForCompletion();

            for (int i = 0; i < myCards.Count; i++)
            {
                Card item = myCards[i];
                item.SetOparcity(0);
            }
        }

        isCardAppearance = !isCardAppearance;
    }

    void SetEcardState()
    {
        if (DrawManager.Instance.isLoading)
            eCardState = ECardState.Loading;

        else if (isCardActivating)
            eCardState = ECardState.ActivatingCard;

        else if (myCards.Count > 0 && !DrawManager.Instance.isLoading)
            eCardState = ECardState.CanUseCard;

        else
            eCardState = ECardState.Noting;
    }

    IEnumerator MouseUpCoroutine()
    {
        mouseExitCoolTime = true;
        myCards.Insert(prevCardNumber, selectDragCard);
        SetOriginOrder(selectDragCard);
        StartCoroutine(CardAlignment());
        selectDragCard = null;

        for (int i = 0; i < myCards.Count; i++)
        {
            Card item = myCards[i];
            item.MouseBlock(false);
        }
        isCardDrag = false;

        yield return delay01;
        mouseExitCoolTime = false;
    }

    IEnumerator AnswerCoroutine()
    {
        mouseExitCoolTime = true;
        selectDragCard.MouseBlock(true);
        myCards.Remove(selectDragCard);

        for (int i = 0; i < myCards.Count; i++)
        {
            Card item = myCards[i];
            item.MouseBlock(false);
        }

        if (!answerObject.GetComponent<Answer>().isInput)
        {
            answerObject.GetComponent<Answer>().InputAnswer(selectDragCard, prevCardNumber);
        }
        else
        {
            answerObject.GetComponent<Answer>().ClearAnswer();
            answerObject.GetComponent<Answer>().InputAnswer(selectDragCard, prevCardNumber);
        }

        selectDragCard.MoveTransform(new PRS(answerObject.transform.position, answerObject.transform.rotation, answerObject.transform.localScale), false);

        selectDragCard = null;
        isCardDrag = false;

        yield return delay01;
        mouseExitCoolTime = false;
    }

    public IEnumerator CardBackCoroutine(Card card, int prevNum)
    {
        mouseExitCoolTime = true;
        myCards.Insert(prevNum, card);
        SetOriginOrder(card);
        StartCoroutine(CardAlignment());

        for (int i = 0; i < myCards.Count; i++)
        {
            Card item = myCards[i];
            item.MouseBlock(false);
        }
        isCardDrag = false;

        yield return delay01;
        mouseExitCoolTime = false;
    }

    #region MyCard

    public void CardMouseOver(Card card)
    {
        if (isCardActivating || eCardState == ECardState.Loading)
            return;

        selectDragCard = card;

        if (!isSelected)
            EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        if (isCardActivating || eCardState == ECardState.Loading)
            return;

        if (isSelected && !isCardDrag && !mouseExitCoolTime)
        {
            EnlargeCard(false, card);
        }
    }

    public void CardMouseDown(Card card)
    {
        if (eCardState == ECardState.Loading)
            return;

        if (canDrag)
        {
            isCardDrag = true;
            selectDragCard.GetComponent<Order>().SetMostFrontOrder(true);

            prevCardNumber = myCards.FindIndex(x => x == card);

            myCards.Remove(card);
            StartCoroutine(CardAlignment());

            for (int i = 0; i < myCards.Count; i++)
            {
                Card item = myCards[i];
                item.MouseBlock(true);
            }
        }
        else
        {
            if (!isCardActivating)
            {
                StartCoroutine(CardSelectAnimation(card));
            }
        }
    }

    public void CardMouseUp()
    {
        if (canDrag)
        {
            if (!canAnswer)
                StartCoroutine(MouseUpCoroutine());
            else
            {
                StartCoroutine(AnswerCoroutine());
            }
        }
    }

    #endregion

}
