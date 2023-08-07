using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonDialogueController : MonoBehaviour
{
    GameObject dialoguePanel;
    Button button;
    bool active;
    bool coolTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetDialogue());

        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnOff());
    }

    IEnumerator GetDialogue()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        dialoguePanel = GameObject.FindGameObjectWithTag("Dialogue");
    }

    void OnOff()
    {
        StartCoroutine(OnOffDialogueCoroutine());
    }

    IEnumerator OnOffDialogueCoroutine()
    {
        button.interactable = false;
        StartCoroutine(GetDialogue());

        if (dialoguePanel == null)
            yield break;

        if (!active)
        {
            // 투명하게



            var tween = dialoguePanel.GetComponent<Image>().DOFade(0, 0.5f);
            dialoguePanel.GetComponentInChildren<TMPro.TMP_Text>().DOFade(0, 0.5f);
            GameObject.FindGameObjectWithTag("Continue")?.SetActive(false);
            GameObject.FindGameObjectWithTag("NameText")?.GetComponent<TMPro.TMP_Text>().DOFade(0, 0.5f);

            dialoguePanel.GetComponent<Yarn.Unity.LineView>().isCardActive = true;
            yield return tween.WaitForCompletion();

            StartCoroutine(CardManager.Instance.CardAlignment());
        }
        else
        {
            // 불투명하게
            var tween = dialoguePanel.GetComponent<Image>().DOFade(1, 0.5f);
            dialoguePanel.GetComponentInChildren<TMPro.TMP_Text>().DOFade(1, 0.5f);
            GameObject.FindGameObjectWithTag("Continue")?.SetActive(true);
            GameObject.FindGameObjectWithTag("NameText")?.GetComponent<TMPro.TMP_Text>().DOFade(1, 0.5f);

            dialoguePanel.GetComponent<Yarn.Unity.LineView>().isCardActive = false;
            yield return tween.WaitForCompletion();
        }
        active = !active;
    }



    public void OnOffDialogue()
    {

    }
}
