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
    private void Start()
    {
        StartCoroutine(StartGameCo());
    }

    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] public int startCardCount = 5;

    [Header("Properties")]
    public bool isLoading; // 카드 사용 방지, 몬스터 공격 방지
    public bool myTurn;

    WaitForSeconds delay025 = new WaitForSeconds(0.15f);

    public static Action<bool> OnAddCard;

    public IEnumerator StartGameCo(float time = 0.5f)
    {
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke(true);
        }

        isLoading = false;
        yield return delay025;

        //CardManager.Instance.TakeOutControlCoroutine();
    }
}