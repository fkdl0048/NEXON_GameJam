using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    Credit,
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
}