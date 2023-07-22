using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 행동 순서 제어
/// </summary>
public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount = 5;

    [Header("Properties")]
    public bool isLoading; // 카드 사용 방지, 몬스터 공격 방지
    public bool myTurn;

    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay035 = new WaitForSeconds(0.35f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public static Action<bool> OnAddCard;
    //public static event Action EndDrawPhase;

    public IEnumerator StartGameCo(float time = 0.5f)
    {
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke(true);
        }

        isLoading = false;
        yield return delay025;

        CardManager.Instance.TakeOutControlCoroutine();
    }

    public IEnumerator ReDrawCards()
    {
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke(true);
            yield return delay025;
        }

        yield return delay035;
        isLoading = false;
    }
}