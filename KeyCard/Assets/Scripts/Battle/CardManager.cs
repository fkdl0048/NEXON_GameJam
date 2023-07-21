using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardUseTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] Transform cardsTransform;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] ECardState eCardState;
    [SerializeField] ItemSO itemSO;

    List<Item> itemBuffer;
    List<Card> shuffleCards;
    string[] keyArray = { "A", "S", "D", "F", "G" };
    Card selectCard;
    int currentCardNumber = -1;
    enum ECardState { Loading, CanUseCard, ActivatingCard, Noting }
    bool isSelected;
    bool isCardActivating;
    bool isMonsterAttack;

    // 카드 사용 시 몬스터에게 알려줄 이벤트
    public static Action<bool> EffectPlayBack;

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
        itemBuffer = new List<Item>(100); // 미리 캐퍼시티 100으로 용량 설정해주면 메모리 효율적으로 사용
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            for (int j = 0; j < item.percent; j++)
                itemBuffer.Add(item);
        }

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void Start()
    {
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
    }

    private void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
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
        InputKey();
    }

    void InputKey()
    {
        if (eCardState == ECardState.CanUseCard)
        {
            if (Input.GetKeyDown(KeyCode.A))
                ChoiceCard(0);
            else if (Input.GetKeyDown(KeyCode.S))
                ChoiceCard(1);
            else if (Input.GetKeyDown(KeyCode.D))
                ChoiceCard(2);
            else if (Input.GetKeyDown(KeyCode.F))
                ChoiceCard(3);
            else if (Input.GetKeyDown(KeyCode.G))
                ChoiceCard(4);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ChoiceCardWithKeyboard(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ChoiceCardWithKeyboard(1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isSelected)
                    StartCoroutine(UseCard());
            }
        }
    }

    void ChoiceCard(int index)
    {
        if (index >= myCards.Count)
            return;

        if (selectCard == myCards[index])
        {
            SelectAndEnlarge(selectCard, true);
            return;
        }
        else
        {
            if (selectCard != null)
                EnlargeCard(false, selectCard);

            SelectAndEnlarge(myCards[index], true);
        }
    }

    void ChoiceCardWithKeyboard(int arrowDirection)
    {
        if (myCards.Count <= 0)
            return;

        if (selectCard != null)
            EnlargeCard(false, selectCard);

        if (myCards.Count == 1 || currentCardNumber == -1)
        {
            SelectAndEnlarge(myCards[0], true);
        }
        else
        {
            var trueNum = currentCardNumber + arrowDirection;

            if (trueNum < 0)
            {
                SelectAndEnlarge(myCards[myCards.Count - 1], true);
                return;
            }
            else if (trueNum >= myCards.Count)
            {
                SelectAndEnlarge(myCards[0], true);
                return;
            }
            SelectAndEnlarge(myCards[trueNum], true);
        }
    }

    void SelectAndEnlarge(Card card, bool isSelecting)
    {
        EnlargeCard(true, card);
        selectCard = card;
        currentCardNumber = myCards.FindIndex(x => x == card);
    }

    public void EnlargeCard(bool isEnlarge, Card card)
    {

        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -6.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Util.QI, Vector3.one * 1.1f), false);
            isSelected = isEnlarge;
        }
        else
        {
            card.MoveTransform(card.originPRS, false);
            isSelected = isEnlarge;
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);

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

    IEnumerator UseCard()
    {
        if (selectCard == null)
            yield break;
        isCardActivating = true;
        EnlargeCard(true, selectCard);
        myCards.Remove(selectCard);

        yield return StartCoroutine(selectCard.MoveTransformCoroutine(new PRS(cardUseTrasnform.position, Util.QI, selectCard.originPRS.scale), true, 0.5f));
        selectCard.DOKill();

        if (myCards.Count == 1)
            currentCardNumber = -1;
        else
            currentCardNumber -= 1;

        if (myCards.Count > 0)
        {
            StartCoroutine(CardAlignment());
            SetKey();
        }
        else
        {
            StartCoroutine(TurnManager.instance.ReDrawCards());
        }

        isCardActivating = false;
        selectCard = null;
    }

    public void SetKey()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].SetKey(keyArray[i]);
        }
    }

    public void DontUseCard(bool isBool)
    {
        isMonsterAttack = isBool;
    }

    void SetEcardState()
    {
        if (TurnManager.instance.isLoading)
            eCardState = ECardState.Loading;

        else if (isCardActivating)
            eCardState = ECardState.ActivatingCard;

        else if (myCards.Count > 0 && !TurnManager.instance.isLoading)
            eCardState = ECardState.CanUseCard;

        else
            eCardState = ECardState.Noting;
    }
}