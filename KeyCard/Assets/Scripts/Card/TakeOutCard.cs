using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TakeOutCard : MonoBehaviour
{
    [SerializeField] GameObject player;

    public void MoveTransform()
    {
        player.transform.DOMoveX(-3.46f, 1f);
    }
}
