using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDialogueController : MonoBehaviour
{
    GameObject[] gameObjects;
    Button button;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetDialogue());

        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnOffDialogue());
    }

    IEnumerator GetDialogue()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameObjects = GameObject.FindGameObjectsWithTag("Dialogue");
    }

    public void OnOffDialogue()
    {
        if (gameObjects == null)
            return;

        foreach (var item in gameObjects)
        {
            item.SetActive(active);
        }
        active = !active;
    }
}
