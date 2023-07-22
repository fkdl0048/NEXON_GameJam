using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Title,
    Dialogue_1,
    Quiz_1,
    Dialogue_2,
    Quiz_2,
    Dialogue_3,
    Quiz_3,
    Dialogue_4,
    Quiz_4,
    HappyEnddingDialogue,
    BadEnddingDialogue,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { Init(); return _instance; } }

    public GameState GameState { get; set; } = GameState.Title;

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<GameManager>();
        }
    }

    private void Start()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Init();

    }

    public void PlusCard()
    {
        if (DrawManager.Instance.isSetting)
        {
            if (DrawManager.Instance.startCardCount < 6)
            {
                DrawManager.Instance.startCardCount += 2;
                for (int i = 0; i < 2; i++)
                    DrawManager.OnAddCard?.Invoke(true);
            }
        }
        else
        {
            if (DrawManager.Instance.startCardCount < 6)
                DrawManager.Instance.startCardCount += 2;
        }
    }

    public void OpenCloseCards()
    {
        if (DrawManager.Instance.isSetting)
            CardManager.Instance.TakeOutCard();
        else
        {
            if (DrawManager.Instance.startCardCount > 0)
            {
                StartCoroutine(GenerateAndOpenCard());
            }
            else
                return;
        }
    }

    IEnumerator GenerateAndOpenCard()
    {
        yield return StartCoroutine(DrawManager.Instance.StartGameCo());
        CardManager.Instance.TakeOutCard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlusCard();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            OpenCloseCards();
        }
    }

    // void InputKey()
    // {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         TurnManager.OnAddCard?.Invoke(true);
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.X))
    //     {
    //         TurnManager.OnAddCard?.Invoke(true);
    //     }
    // }
}