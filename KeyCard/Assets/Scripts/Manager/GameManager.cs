using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { Init(); return _instance; } }
    
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
        StartCoroutine(TurnManager.instance.StartGameCo());
    }

    private void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TurnManager.OnAddCard?.Invoke(true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnManager.OnAddCard?.Invoke(true);
        }
    }
}