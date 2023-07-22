using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    void Start()
    {
        fadeController.FadeOut();
        
        Debug.Log("!@#?");
    }


    public void Do()
    {
        Debug.Log("Do");
    }
}
