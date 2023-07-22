using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    
    void Start()
    {
        fadeController.FadeOut();
    }
}
