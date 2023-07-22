using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : MonoBehaviour
{
    [SerializeField] Transform firstArea;
    [SerializeField] Transform secondArea;
    [SerializeField] Sprite cardFrontSprite;
    //List<Card> deckCardList;

    Image[] firstAreaImages;
    Image[] secondAreaImages;

    int count = 1;

    void Start()
    {
        //deckCardList = new List<Card>();

        firstAreaImages = firstArea.GetComponentsInChildren<Image>();
        secondAreaImages = secondArea.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void PlusCard()
    {
        if (count <= 3)
        {
            firstAreaImages[count].sprite = cardFrontSprite;
            secondAreaImages[count].sprite = cardFrontSprite;
            count++;
        }
        else
            return;
    }
}
