/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class BattleCardManager : MonoBehaviour
{

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardHandTransform;
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardUseTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] List<BattleCard> myCards;
    [SerializeField] ECardState eCardState;

    Dictionary<int, BattleCard> deckCards;
    List<BattleCard> shuffleCards;
    string[] keyArray = { "A", "S", "D", "F", "G" };
    BattleCard selectCard;
    int currentCardNumber = -1;
    enum ECardState { Loading, CanUseCard, ActivatingCard, Noting }
    bool isSelected;
    bool isCardActivating;
    bool isMonsterAttack;

    // 카드 사용 시 몬스터에게 알려줄 이벤트
    public static Action<bool> EffectPlayBack;

    public BattleCard PopCard()
    {
        if (shuffleCards.Count == 0)
            SetupCardBuffer();

        BattleCard deckCard = shuffleCards[0];
        shuffleCards.RemoveAt(0);
        return deckCard;
    }

    void SetupCardBuffer()
    {
        shuffleCards = new List<DeckCard>(100);
        deckCards = Managers.Data.CardDict;
        unlockCards = Managers.Data.DeckDict;

        foreach (KeyValuePair<int, DeckCard> deck in deckCards)
        {
            foreach (KeyValuePair<int, UnlockCard> unlock in unlockCards)
            {
                if (deck.Key == unlock.Value.index)
                {
                    for (int i = 0; i < deck.Value.percent; i++)
                    {
                        shuffleCards.Add(deck.Value);
                    }
                }
            }
        }

        for (int i = 0; i < shuffleCards.Count; i++)
        {
            int rand = Random.Range(i, shuffleCards.Count);
            DeckCard temp = shuffleCards[i];
            shuffleCards[i] = shuffleCards[rand];
            shuffleCards[rand] = temp;
        }
    }

    void Start()
    {
        SetupCardBuffer();
        BattleTurnManager.OnAddCard += AddCard;
        //BattleTurnManager.EndDrawPhase += EndDrawPhase;
    }

    void OnDestroy()
    {
        BattleTurnManager.OnAddCard -= AddCard;
        //BattleTurnManager.EndDrawPhase -= EndDrawPhase;
    }

    public void CreatePoolCard()
    {
        Managers.Pool.CreatePool(cardPrefab,10);
    }

    void AddCard()
    {
        //var cardGeneratePositon = cardsParentTransform.position + Vector3.up * -4.5f;

        var cardObject = Managers.Pool.Pop(cardPrefab, cardHandTransform);
        cardObject.transform.position = cardSpawnTrasnform.position;

        var card = cardObject.GetComponent<BattleCard>();
        card.Setup(PopCard());
        myCards.Add(card);

        SetOriginOrder();
        StartCoroutine(CardAlignment());
    }

    void SetOriginOrder()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(myCards.Count-i);
        }
    }

*//*    void EndDrawPhase()
    {
        // 5장 드로우가 끝나고 실행될 함수
    }*//*

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

    void SelectAndEnlarge(BattleCard card, bool isSelecting)
    {
        EnlargeCard(true, card);
        selectCard = card;
        currentCardNumber = myCards.FindIndex(x => x == card);
    }

    public void EnlargeCard(bool isEnlarge, BattleCard card)
    {

        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -6.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.1f), false);
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
        originCardPRSs = RoundAlignment(cardLeft, cardRight, myCards.Count, 0.5f, Vector3.one * 1);

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
            var targetRot = Utils.QI;
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
        EffectPlayBack?.Invoke(true);
        myCards.Remove(selectCard);

        yield return StartCoroutine(selectCard.MoveTransformCoroutine(new PRS(cardUseTrasnform.position, Utils.QI, selectCard.originPRS.scale), true, 0.5f));
        BattleMagicManager.instance.CallMagic(selectCard.deckCard);
        selectCard.DOKill();
        Managers.Pool.Push(selectCard.GetComponent<Poolable>());

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
            StartCoroutine(BattleTurnManager.instance.ReDrawCards());
        }

        isCardActivating = false;
        EffectPlayBack?.Invoke(false);
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
        if (BattleTurnManager.instance.isLoading || isMonsterAttack)
            eCardState = ECardState.Loading;

        else if (isCardActivating)
            eCardState = ECardState.ActivatingCard;

        else if (myCards.Count > 0 && !BattleTurnManager.instance.isLoading)
            eCardState = ECardState.CanUseCard;

        else
            eCardState = ECardState.Noting;
    }
}
*/